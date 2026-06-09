using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class RelatoriosController : BaseController
{
    private readonly AppDbContext _db;

    public RelatoriosController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(DateTime? inicio, DateTime? fim, string? status, int? responsavelId, string? vinculo)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        // Filtro padrão = mês atual (dia 1 até hoje)
        var hoje = DateTime.UtcNow;
        if (!inicio.HasValue && !fim.HasValue && string.IsNullOrWhiteSpace(status)
            && !responsavelId.HasValue && string.IsNullOrWhiteSpace(vinculo))
        {
            inicio = new DateTime(hoje.Year, hoje.Month, 1);
            fim    = hoje.Date;
        }

        var query = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Responsavel)
            .AsQueryable();

        if (inicio.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(inicio.Value.Date, DateTimeKind.Utc);
            query = query.Where(o => o.DataAbertura >= inicioUtc);
        }
        if (fim.HasValue)
        {
            var fimUtc = DateTime.SpecifyKind(fim.Value.Date.AddDays(1), DateTimeKind.Utc);
            query = query.Where(o => o.DataAbertura < fimUtc);
        }
        if (!string.IsNullOrWhiteSpace(status))      query = query.Where(o => o.Status == status);
        if (responsavelId.HasValue)                  query = query.Where(o => o.ResponsavelId == responsavelId);
        if (!string.IsNullOrWhiteSpace(vinculo))     query = query.Where(o => o.Equipamento.Vinculo == vinculo);

        var ordens = query.OrderByDescending(o => o.DataAbertura).ToList();
        var ids    = ordens.Select(o => o.Id).ToList();

        var agora = DateTime.UtcNow;
        var registros = _db.RegistrosTempo
            .Where(r => ids.Contains(r.OrdemServicoId))
            .ToList();

        var emExecucaoIds = ordens
            .Where(o => o.Status == "Em Execucao")
            .Select(o => o.Id)
            .ToHashSet();

        // Tempo líquido por OS
        var temposPorOS = ids.ToDictionary(id => id, id =>
        {
            var regs = registros.Where(r => r.OrdemServicoId == id);
            return regs.Aggregate(TimeSpan.Zero, (acc, r) =>
            {
                DateTime? f = r.Fim;
                if (f == null)
                {
                    if (!emExecucaoIds.Contains(id)) return acc;
                    f = agora;
                }
                var dur = f.Value - r.Inicio;
                return dur > TimeSpan.Zero ? acc + dur : acc;
            });
        });

        // ── KPIs ────────────────────────────────────────────────────
        var totalAbertas    = ordens.Count;
        var totalConcluidas = ordens.Count(o => o.Status == "Concluida");
        var horasLiquidas   = temposPorOS.Values.Aggregate(TimeSpan.Zero, (a, t) => a + t);

        var concluidasComTempo = ordens
            .Where(o => o.Status == "Concluida" && temposPorOS[o.Id] > TimeSpan.Zero)
            .ToList();
        var mttr = concluidasComTempo.Count > 0
            ? TimeSpan.FromTicks(
                concluidasComTempo.Select(o => temposPorOS[o.Id].Ticks)
                    .Aggregate(0L, (a, t) => a + t) / concluidasComTempo.Count)
            : TimeSpan.Zero;

        // ── Chart 1: Laboratórios com mais falhas ────────────────────
        var chartLabs = ordens
            .GroupBy(o => string.IsNullOrWhiteSpace(o.Equipamento.Localizacao)
                ? "Não informado" : o.Equipamento.Localizacao)
            .Select(g => new { local = g.Key, count = g.Count() })
            .OrderByDescending(x => x.count)
            .Take(8)
            .ToList();

        // ── Chart 2: Horas por Vínculo ───────────────────────────────
        var chartVinculo = ordens
            .GroupBy(o => string.IsNullOrWhiteSpace(o.Equipamento.Vinculo)
                ? "Não informado" : o.Equipamento.Vinculo)
            .Select(g => new
            {
                vinculo = g.Key,
                horas   = g.Sum(o => temposPorOS[o.Id].TotalHours)
            })
            .OrderByDescending(x => x.horas)
            .ToList();

        // ── Chart 3: Horas + stats por responsável ───────────────────
        var chartResp = ordens
            .Where(o => o.Responsavel != null)
            .GroupBy(o => o.Responsavel!.Nome)
            .Select(g => new
            {
                nome          = g.Key,
                horas         = g.Sum(o => temposPorOS[o.Id].TotalHours),
                osConcluidas  = g.Count(o => o.Status == "Concluida"),
                mediaHoras    = g.Count() > 0
                    ? g.Sum(o => temposPorOS[o.Id].TotalHours) / g.Count()
                    : 0.0
            })
            .OrderByDescending(x => x.horas)
            .ToList();

        // ── Chart 4: Ativos (com NI) com mais recorrência ────────────
        var chartAtivos = ordens
            .Where(o => !string.IsNullOrWhiteSpace(o.Equipamento.NI))
            .GroupBy(o => $"{o.Equipamento.NI} — {o.Equipamento.Nome}")
            .Select(g => new { ativo = g.Key, count = g.Count() })
            .OrderByDescending(x => x.count)
            .Take(10)
            .ToList();

        // ── ViewBag ──────────────────────────────────────────────────
        ViewBag.TotalAbertas    = totalAbertas;
        ViewBag.TotalConcluidas = totalConcluidas;
        ViewBag.HorasLiquidas   = horasLiquidas;
        ViewBag.Mttr            = mttr;

        ViewBag.Usuarios = _db.Usuarios.OrderBy(u => u.Nome).ToList();
        ViewBag.Vinculos = _db.Equipamentos
            .Where(e => e.Vinculo != null && e.Vinculo != "")
            .Select(e => e.Vinculo!).Distinct().OrderBy(v => v).ToList();

        ViewBag.FiltroInicio      = inicio?.ToString("yyyy-MM-dd");
        ViewBag.FiltroFim         = fim?.ToString("yyyy-MM-dd");
        ViewBag.FiltroStatus      = status;
        ViewBag.FiltroResponsavel = responsavelId;
        ViewBag.FiltroVinculo     = vinculo;

        // Tabela de responsáveis (para exibir abaixo do gráfico 3)
        ViewBag.RespStats = chartResp;

        var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        ViewBag.ChartLabsJson    = JsonSerializer.Serialize(chartLabs,    opts);
        ViewBag.ChartVinculoJson = JsonSerializer.Serialize(chartVinculo, opts);
        ViewBag.ChartRespJson    = JsonSerializer.Serialize(chartResp,    opts);
        ViewBag.ChartAtivosJson  = JsonSerializer.Serialize(chartAtivos,  opts);

        return View(ordens);
    }

    public IActionResult ExportarCsv(DateTime? inicio, DateTime? fim, string? status, int? responsavelId, string? vinculo)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var hoje = DateTime.UtcNow;
        if (!inicio.HasValue && !fim.HasValue && string.IsNullOrWhiteSpace(status)
            && !responsavelId.HasValue && string.IsNullOrWhiteSpace(vinculo))
        {
            inicio = new DateTime(hoje.Year, hoje.Month, 1);
            fim    = hoje.Date;
        }

        var query = _db.OrdensServico
            .Include(o => o.Equipamento)
            .Include(o => o.Responsavel)
            .AsQueryable();

        if (inicio.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(inicio.Value.Date, DateTimeKind.Utc);
            query = query.Where(o => o.DataAbertura >= inicioUtc);
        }
        if (fim.HasValue)
        {
            var fimUtc = DateTime.SpecifyKind(fim.Value.Date.AddDays(1), DateTimeKind.Utc);
            query = query.Where(o => o.DataAbertura < fimUtc);
        }
        if (!string.IsNullOrWhiteSpace(status))  query = query.Where(o => o.Status == status);
        if (responsavelId.HasValue)               query = query.Where(o => o.ResponsavelId == responsavelId);
        if (!string.IsNullOrWhiteSpace(vinculo))  query = query.Where(o => o.Equipamento.Vinculo == vinculo);

        var ordens    = query.OrderByDescending(o => o.DataAbertura).ToList();
        var ids       = ordens.Select(o => o.Id).ToList();
        var registros = _db.RegistrosTempo.Where(r => ids.Contains(r.OrdemServicoId)).ToList();
        var emExecIds = ordens.Where(o => o.Status == "Em Execucao").Select(o => o.Id).ToHashSet();
        var agora     = DateTime.UtcNow;

        TimeSpan CalcTempo(int id) =>
            registros.Where(r => r.OrdemServicoId == id)
                .Aggregate(TimeSpan.Zero, (acc, r) =>
                {
                    DateTime? f = r.Fim ?? (emExecIds.Contains(id) ? agora : (DateTime?)null);
                    if (f == null) return acc;
                    var dur = f.Value - r.Inicio;
                    return dur > TimeSpan.Zero ? acc + dur : acc;
                });

        static string FmtTs(TimeSpan t) => $"{(int)t.TotalHours:00}h{t.Minutes:00}";
        static string Esc(string? s)
        {
            if (s is null) return "";
            return s.Contains(';') || s.Contains('"') || s.Contains('\n')
                ? $"\"{s.Replace("\"", "\"\"")}\"" : s;
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("OS;Equipamento;NI;Localizacao;Vinculo;Responsavel;Status;Abertura;Tempo Trabalhado");
        foreach (var o in ordens)
        {
            sb.AppendLine(string.Join(";", new[]
            {
                Esc($"OS{o.Id:000000}"),
                Esc(o.Equipamento.Nome),
                Esc(o.Equipamento.NI ?? ""),
                Esc(o.Equipamento.Localizacao ?? ""),
                Esc(o.Equipamento.Vinculo ?? ""),
                Esc(o.Responsavel?.Nome ?? "Não designado"),
                Esc(o.Status),
                Esc(o.DataAbertura.ToLocalTime().ToString("dd/MM/yyyy HH:mm")),
                FmtTs(CalcTempo(o.Id))
            }));
        }

        var nomeArquivo = $"relatorio_{inicio?.ToString("yyyyMMdd") ?? "inicio"}_{fim?.ToString("yyyyMMdd") ?? "fim"}.csv";
        var bom   = System.Text.Encoding.UTF8.GetPreamble();
        var corpo = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        var bytes = new byte[bom.Length + corpo.Length];
        bom.CopyTo(bytes, 0);
        corpo.CopyTo(bytes, bom.Length);
        return File(bytes, "text/csv; charset=utf-8", nomeArquivo);
    }
}
