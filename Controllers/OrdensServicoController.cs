using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class OrdensServicoController : BaseController
{
    private readonly AppDbContext _db;

    public OrdensServicoController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(string? busca, string? status, string? local,
                               int? responsavelId, string? ordemTempo, int page = 1)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        const int PageSize = 10;

        var query = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Solicitante)
            .Include(o => o.Responsavel)
            .Where(o => IsAdmin || o.SolicitanteId == UsuarioId || o.ResponsavelId == UsuarioId)
            .AsQueryable();

        // Busca case-insensitive em nome e NI
        if (!string.IsNullOrWhiteSpace(busca))
        {
            var b = busca.ToLower();
            query = query.Where(o =>
                o.Equipamento.Nome.ToLower().Contains(b) ||
                (o.Equipamento.NI != null && o.Equipamento.NI.ToLower().Contains(b)));
        }

        // Status: default exclui Concluída
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(o => o.Status == status);
        else
            query = query.Where(o => o.Status != "Concluida");

        if (!string.IsNullOrWhiteSpace(local))
            query = query.Where(o => o.Equipamento.Localizacao != null &&
                                     o.Equipamento.Localizacao.ToLower().Contains(local.ToLower()));

        if (responsavelId.HasValue)
            query = query.Where(o => o.ResponsavelId == responsavelId);

        // Ordenação por data: antigas (default) ou recentes
        query = ordemTempo == "recentes"
            ? query.OrderByDescending(o => o.DataAbertura)
            : query.OrderBy(o => o.DataAbertura);

        var totalRegistros = query.Count();
        var totalPaginas   = (int)Math.Ceiling(totalRegistros / (double)PageSize);
        page = Math.Clamp(page, 1, Math.Max(1, totalPaginas));

        var ordens = query.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var ids   = ordens.Select(o => o.Id).ToList();
        var agora = DateTime.UtcNow;
        var registros = _db.RegistrosTempo.Where(r => ids.Contains(r.OrdemServicoId)).ToList();
        var emExecIds  = ordens.Where(o => o.Status == "Em Execucao").Select(o => o.Id).ToHashSet();

        ViewBag.TemposPorOS = ids.ToDictionary(id => id, id =>
        {
            var regs     = registros.Where(r => r.OrdemServicoId == id);
            var timerAtivo = regs.Any(r => r.Fim == null && emExecIds.Contains(id));
            var total = regs.Aggregate(TimeSpan.Zero, (acc, r) =>
            {
                var fim = (r.Fim == null && emExecIds.Contains(id)) ? agora : r.Fim;
                if (fim == null) return acc;
                var dur = fim.Value - r.Inicio;
                return dur > TimeSpan.Zero ? acc + dur : acc;
            });
            return (Tempo: total, Ativo: timerAtivo);
        });

        ViewBag.IsAdmin       = IsAdmin;
        ViewBag.Busca         = busca;
        ViewBag.StatusFiltro  = status;
        ViewBag.Local         = local;
        ViewBag.ResponsavelId = responsavelId;
        ViewBag.OrdemTempo    = ordemTempo;
        ViewBag.Page          = page;
        ViewBag.TotalPaginas  = totalPaginas;
        ViewBag.TotalRegistros = totalRegistros;
        ViewBag.Locais = _db.Equipamentos
            .Where(e => e.Localizacao != null && e.Localizacao != "")
            .Select(e => e.Localizacao!)
            .Distinct().OrderBy(l => l).ToList();
        ViewBag.Usuarios = _db.Usuarios.OrderBy(u => u.Nome).ToList();
        return View(ordens);
    }

    public IActionResult Abrir()
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        return View();
    }

    [HttpPost]
    public IActionResult Salvar(bool possuiNI, string? ni, string? nomeEquipamento, string? localizacao, string? vinculo, string? tipoProblema, string descricaoProblema, bool euMesmoVouExecutar)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        if (string.IsNullOrWhiteSpace(descricaoProblema))
        {
            TempData["Erro"] = "Descreva o problema antes de continuar.";
            return RedirectToAction("Abrir");
        }

        Equipamento? equipamento = null;

        if (possuiNI)
        {
            equipamento = _db.Equipamentos.FirstOrDefault(e => e.NI == ni && e.Ativo);
            if (equipamento is null)
            {
                TempData["Erro"] = "Equipamento com esse NI nao foi encontrado.";
                return RedirectToAction("Abrir");
            }

            var osAberta = _db.OrdensServico
                .FirstOrDefault(o => o.EquipamentoId == equipamento.Id && o.Status != "Concluida");
            if (osAberta is not null)
            {
                TempData["Erro"] = $"Este ativo já possui a OS #{osAberta.Id} em aberto (status: {osAberta.Status}). Conclua a OS atual antes de abrir uma nova.";
                return RedirectToAction("Abrir");
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(nomeEquipamento))
            {
                TempData["Erro"] = "Informe o nome do equipamento.";
                return RedirectToAction("Abrir");
            }

            equipamento = new Equipamento
            {
                PossuiNI = false,
                Nome = nomeEquipamento,
                Localizacao = localizacao,
                Vinculo = string.IsNullOrWhiteSpace(vinculo) ? null : vinculo
            };
            _db.Equipamentos.Add(equipamento);
        }

        var ordem = new OrdemServico
        {
            Equipamento = equipamento,
            SolicitanteId = UsuarioId!.Value,
            ResponsavelId = euMesmoVouExecutar ? UsuarioId : null,
            TipoProblema = tipoProblema,
            DescricaoProblema = descricaoProblema,
            Status = euMesmoVouExecutar ? "Em Execucao" : "Pendente"
        };

        _db.OrdensServico.Add(ordem);
        _db.SaveChanges();

        if (euMesmoVouExecutar)
        {
            _db.RegistrosTempo.Add(new RegistroTempo
            {
                OrdemServicoId = ordem.Id,
                ResponsavelId = UsuarioId!.Value,
                Inicio = DateTime.UtcNow
            });
            _db.SaveChanges();
        }

        RegistrarHistorico(ordem.Id, euMesmoVouExecutar ? "Execucao iniciada pelo solicitante" : "OS enviada para OPP");

        if (euMesmoVouExecutar)
        {
            return RedirectToAction("Detalhes", "Execucao", new { id = ordem.Id });
        }

        TempData["Mensagem"] = "Solicitacao enviada para a OPP. Voce pode acompanhar o andamento na tela inicial.";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Detalhes(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        var ordem = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Solicitante)
            .Include(o => o.Responsavel)
            .FirstOrDefault(o => o.Id == id);

        if (ordem is null) return NotFound();
        if (!IsAdmin && ordem.SolicitanteId != UsuarioId && ordem.ResponsavelId != UsuarioId) return RedirectToAction("Index", "Home");

        ViewBag.Historico = _db.HistoricosOS
            .Include(h => h.Usuario)
            .Where(h => h.OrdemServicoId == id)
            .OrderByDescending(h => h.DataHora)
            .ToList();

        ViewBag.Tempo = CalcularTempo(id);
        return View(ordem);
    }

    public IActionResult Designar(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var ordem = _db.OrdensServico.Include(o => o.Equipamento).FirstOrDefault(o => o.Id == id);
        if (ordem is null) return NotFound();

        ViewBag.Usuarios = _db.Usuarios.OrderBy(u => u.Nome).ToList();
        return View(ordem);
    }

    [HttpPost]
    public IActionResult SalvarDesignacao(int id, int responsavelId)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var ordem = _db.OrdensServico.Find(id);
        if (ordem is null) return NotFound();

        ordem.ResponsavelId = responsavelId;
        ordem.Status = "Em Execucao";
        _db.SaveChanges();
        RegistrarHistorico(ordem.Id, "Responsavel designado");

        TempData["Mensagem"] = "Responsavel designado com sucesso.";
        return RedirectToAction("Index");
    }

    private TimeSpan CalcularTempo(int ordemId)
    {
        var agora = DateTime.UtcNow;
        return _db.RegistrosTempo
            .Where(r => r.OrdemServicoId == ordemId)
            .AsEnumerable()
            .Aggregate(TimeSpan.Zero, (total, registro) => total + ((registro.Fim ?? agora) - registro.Inicio));
    }

    private void RegistrarHistorico(int ordemId, string acao, string? observacao = null)
    {
        _db.HistoricosOS.Add(new HistoricoOS
        {
            OrdemServicoId = ordemId,
            UsuarioId = UsuarioId!.Value,
            Acao = acao,
            Observacao = observacao
        });
        _db.SaveChanges();
    }
}
