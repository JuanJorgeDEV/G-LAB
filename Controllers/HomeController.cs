using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class HomeController : BaseController
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(string? statusFiltro)
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        ViewBag.UsuarioNome   = UsuarioNome;
        ViewBag.UsuarioPerfil = UsuarioPerfil;
        ViewBag.StatusFiltro  = statusFiltro;

        if (IsAdmin)
            return IndexAdmin();

        return IndexColaborador(statusFiltro);
    }

    // ── Painel do colaborador ────────────────────────────────────────
    private IActionResult IndexColaborador(string? statusFiltro)
    {
        var baseQuery = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Responsavel)
            .Where(o => o.SolicitanteId == UsuarioId || o.ResponsavelId == UsuarioId);

        // Métricas pessoais (excluindo Concluída para manter contagens úteis)
        ViewBag.EmExecucao = baseQuery.Count(o => o.Status == "Em Execucao");
        ViewBag.Aguardando = baseQuery.Count(o => o.Status == "Aguardando");
        ViewBag.Pendentes  = baseQuery.Count(o => o.Status == "Pendente");
        ViewBag.Concluidas = baseQuery.Count(o => o.Status == "Concluida");

        // Lista filtrada — por padrão exclui Concluídas
        var listaQuery = baseQuery;
        if (!string.IsNullOrWhiteSpace(statusFiltro))
            listaQuery = listaQuery.Where(o => o.Status == statusFiltro);
        else
            listaQuery = listaQuery.Where(o => o.Status != "Concluida");

        var ordens = listaQuery
            .OrderByDescending(o => o.DataAbertura)
            .ToList();

        // Tempo acumulado
        var ids = ordens.Select(o => o.Id).ToList();
        var agora = DateTime.UtcNow;
        var registros = _db.RegistrosTempo.Where(r => ids.Contains(r.OrdemServicoId)).ToList();
        var emExecIds = ordens.Where(o => o.Status == "Em Execucao").Select(o => o.Id).ToHashSet();

        var temposPorOS = ids.ToDictionary(id => id, id =>
        {
            var regs = registros.Where(r => r.OrdemServicoId == id);
            var ativo = regs.Any(r => r.Fim == null && emExecIds.Contains(id));
            var total = regs.Aggregate(TimeSpan.Zero, (acc, r) =>
            {
                var fim = (r.Fim == null && emExecIds.Contains(id)) ? agora : r.Fim;
                if (fim == null) return acc;
                var dur = fim.Value - r.Inicio;
                return dur > TimeSpan.Zero ? acc + dur : acc;
            });
            return (Tempo: total, Ativo: ativo);
        });

        // OS com timer ativo sobem ao topo
        var ordenadas = ordens
            .OrderByDescending(o => temposPorOS[o.Id].Ativo)
            .ThenByDescending(o => o.DataAbertura)
            .ToList();

        ViewBag.Minhas     = ordenadas;
        ViewBag.TemposPorOS = temposPorOS;
        return View();
    }

    // ── Painel da OPP (admin) ────────────────────────────────────────
    private IActionResult IndexAdmin()
    {
        // KPIs globais
        ViewBag.Pendentes   = _db.OrdensServico.Count(o => o.Status == "Pendente");
        ViewBag.EmExecucao  = _db.OrdensServico.Count(o => o.Status == "Em Execucao");
        ViewBag.Aguardando  = _db.OrdensServico.Count(o => o.Status == "Aguardando");
        ViewBag.Concluidas  = _db.OrdensServico.Count(o => o.Status == "Concluida");

        // Seção 1: top 5 OS pendentes (mais antigas)
        ViewBag.PendentesLista = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Solicitante)
            .Where(o => o.Status == "Pendente")
            .OrderBy(o => o.DataAbertura)
            .Take(5)
            .ToList();

        // Seção 2: top 5 OS em status Aguardando há mais tempo
        ViewBag.ParadasLista = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Responsavel)
            .Where(o => o.Status == "Aguardando")
            .OrderBy(o => o.DataAbertura)
            .Take(5)
            .ToList();

        return View();
    }

    public IActionResult Perfil()
    {
        var login = ExigirLogin();
        if (login is not null) return login;

        var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == UsuarioId);
        return usuario is null ? RedirectToAction("Index", "Login") : View(usuario);
    }

    public IActionResult Ajuda()
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
