using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class ExecucaoController : BaseController
{
    private readonly AppDbContext _db;

    public ExecucaoController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Detalhes(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        var ordem = CarregarOrdem(id);
        if (ordem is null) return NotFound();

        if (!IsAdmin && !PodeExecutar(ordem)) return RedirectToAction("Index", "Home");

        var tempoAcumulado = CalcularTempo(id);
        var tempoAberto = _db.RegistrosTempo.Any(r => r.OrdemServicoId == id && r.Fim == null);

        ViewBag.Tempo = tempoAcumulado;
        ViewBag.TempoSegundos = (int)tempoAcumulado.TotalSeconds;
        ViewBag.TempoAberto = tempoAberto;
        ViewBag.PodeAtuar = PodeExecutar(ordem);
        return View(ordem);
    }

    // Mantido para o caso de OS designada pela OPP (ainda sem RegistroTempo)
    public IActionResult Iniciar(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        return AbrirRegistro(id, "Execucao iniciada");
    }

    [HttpPost]
    public IActionResult Pausar(int id, string motivo)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        var ordem = _db.OrdensServico.Find(id);
        if (ordem is null) return NotFound();
        if (!PodeExecutar(ordem)) return RedirectToAction("Index", "Home");

        FecharRegistrosAbertos(id);
        ordem.Status = "Aguardando";
        RegistrarHistorico(id, "Trabalho pausado", string.IsNullOrWhiteSpace(motivo) ? null : motivo);
        _db.SaveChanges();
        return RedirectToAction("Detalhes", new { id });
    }

    public IActionResult Continuar(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        return AbrirRegistro(id, "Trabalho retomado");
    }

    [HttpPost]
    public IActionResult Finalizar(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        var ordem = _db.OrdensServico.Find(id);
        if (ordem is null) return NotFound();
        if (!PodeExecutar(ordem)) return RedirectToAction("Index", "Home");

        FecharRegistrosAbertos(id);
        ordem.Status = "Concluida";
        ordem.DataConclusao = DateTime.UtcNow;
        RegistrarHistorico(id, "OS concluida");
        _db.SaveChanges();
        return RedirectToAction("Detalhes", new { id });
    }

    private IActionResult AbrirRegistro(int id, string acao)
    {
        var ordem = _db.OrdensServico.Find(id);
        if (ordem is null) return NotFound();
        if (!PodeExecutar(ordem)) return RedirectToAction("Index", "Home");

        bool jaTemEmExecucao = _db.OrdensServico.Any(o => o.ResponsavelId == UsuarioId && o.Status == "Em Execucao" && o.Id != id);
        if (jaTemEmExecucao)
        {
            TempData["Erro"] = "Não é possível iniciar esta ordem pois você já possui outra ordem de serviço em execução. Pause ou conclua a ordem atual primeiro.";
            return RedirectToAction("Detalhes", new { id });
        }

        // Garante que não há registro aberto duplicado antes de criar um novo
        if (!_db.RegistrosTempo.Any(r => r.OrdemServicoId == id && r.Fim == null))
        {
            _db.RegistrosTempo.Add(new RegistroTempo
            {
                OrdemServicoId = id,
                ResponsavelId = UsuarioId!.Value,
                Inicio = DateTime.UtcNow
            });
        }

        ordem.Status = "Em Execucao";
        RegistrarHistorico(id, acao);
        _db.SaveChanges();
        return RedirectToAction("Detalhes", new { id });
    }

    private OrdemServico? CarregarOrdem(int id)
    {
        return _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Solicitante)
            .Include(o => o.Responsavel)
            .FirstOrDefault(o => o.Id == id);
    }

    private bool PodeExecutar(OrdemServico ordem)
    {
        return ordem.ResponsavelId == UsuarioId;
    }

    // Fecha TODOS os registros abertos (segurança contra dados inconsistentes)
    private void FecharRegistrosAbertos(int ordemId)
    {
        var abertos = _db.RegistrosTempo
            .Where(r => r.OrdemServicoId == ordemId && r.Fim == null)
            .ToList();
        foreach (var r in abertos)
            r.Fim = DateTime.UtcNow;
    }

    private TimeSpan CalcularTempo(int ordemId)
    {
        var agora = DateTime.UtcNow;
        // Soma apenas os intervalos fechados (Fim != null) + o aberto atual se houver
        // Intervalo inválido (Fim <= Inicio) é ignorado para não distorcer o total
        return _db.RegistrosTempo
            .Where(r => r.OrdemServicoId == ordemId)
            .AsEnumerable()
            .Aggregate(TimeSpan.Zero, (total, r) =>
            {
                var fim = r.Fim ?? agora;
                var duracao = fim - r.Inicio;
                return duracao > TimeSpan.Zero ? total + duracao : total;
            });
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
    }
}
