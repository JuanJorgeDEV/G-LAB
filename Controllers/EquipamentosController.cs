using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class EquipamentosController : BaseController
{
    private readonly AppDbContext _db;

    public EquipamentosController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(string? busca)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var equipamentos = _db.Equipamentos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca))
        {
            equipamentos = equipamentos.Where(e =>
                (e.NI != null && e.NI.Contains(busca)) ||
                e.Nome.Contains(busca) ||
                (e.Localizacao != null && e.Localizacao.Contains(busca)));
        }

        ViewBag.Busca = busca;
        ViewBag.EmManutencao = _db.OrdensServico
            .Where(o => o.Status != "Concluida")
            .Select(o => o.EquipamentoId)
            .ToHashSet();
        return View(equipamentos.OrderBy(e => e.Nome).ToList());
    }

    public IActionResult Cadastrar()
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        return View("Form", new Equipamento());
    }

    public IActionResult Editar(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var equipamento = _db.Equipamentos.Find(id);
        return equipamento is null ? NotFound() : View("Form", equipamento);
    }

    [HttpPost]
    public IActionResult Salvar(Equipamento equipamento)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        if (equipamento.PossuiNI && string.IsNullOrWhiteSpace(equipamento.NI))
        {
            ModelState.AddModelError(nameof(equipamento.NI), "Informe o NI.");
        }

        var niEmUso = !string.IsNullOrWhiteSpace(equipamento.NI) &&
            _db.Equipamentos.Any(e => e.NI == equipamento.NI && e.Id != equipamento.Id);

        if (niEmUso)
        {
            ModelState.AddModelError(nameof(equipamento.NI), "Ja existe equipamento com este NI.");
        }

        if (!ModelState.IsValid) return View("Form", equipamento);

        equipamento.NI = equipamento.PossuiNI ? equipamento.NI : null;

        if (equipamento.Id == 0)
        {
            _db.Equipamentos.Add(equipamento);
        }
        else
        {
            _db.Equipamentos.Update(equipamento);
        }

        _db.SaveChanges();
        TempData["Mensagem"] = "Equipamento salvo com sucesso.";
        return RedirectToAction("Index");
    }

    public IActionResult HistoricoOS(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return Unauthorized();
        if (!IsAdmin) return Forbid();

        var ordens = _db.OrdensServico
            .Include(o => o.Responsavel)
            .Where(o => o.EquipamentoId == id)
            .OrderByDescending(o => o.DataAbertura)
            .Select(o => new
            {
                id = o.Id,
                dataAbertura = o.DataAbertura.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                descricao = o.DescricaoProblema,
                status = o.Status,
                responsavel = o.Responsavel != null ? o.Responsavel.Nome : "Não designado"
            })
            .ToList();

        return Json(ordens);
    }

    public IActionResult BuscarPorNI(string ni)
    {
        var equipamento = _db.Equipamentos.AsNoTracking().FirstOrDefault(e => e.NI == ni && e.Ativo);
        if (equipamento is null) return NotFound();

        var osAberta = _db.OrdensServico
            .Where(o => o.EquipamentoId == equipamento.Id && o.Status != "Concluida")
            .Select(o => new { o.Id, o.Status })
            .FirstOrDefault();

        return Json(new
        {
            id = equipamento.Id,
            nome = equipamento.Nome,
            localizacao = equipamento.Localizacao,
            vinculo = equipamento.Vinculo,
            osAberta = osAberta != null,
            osAbertaId = osAberta != null ? (int?)osAberta.Id : null,
            osAbertaStatus = osAberta?.Status
        });
    }
}
