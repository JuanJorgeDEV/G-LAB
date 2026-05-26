using Microsoft.AspNetCore.Mvc;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class FuncionariosController : BaseController
{
    private readonly AppDbContext _db;

    public FuncionariosController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var usuarios = _db.Usuarios.OrderBy(u => u.Nome).ToList();
        return View(usuarios);
    }

    public IActionResult Cadastrar()
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        return View(new Usuario { Perfil = "Colaborador" });
    }

    [HttpPost]
    public IActionResult Salvar(Usuario usuario)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        usuario.Email = usuario.Email.Trim().ToLowerInvariant();
        usuario.CPF = FormatCpf(usuario.CPF);

        if (string.IsNullOrWhiteSpace(usuario.Nome))
        {
            ModelState.AddModelError(nameof(usuario.Nome), "Informe o nome.");
        }

        if (string.IsNullOrWhiteSpace(usuario.Email))
        {
            ModelState.AddModelError(nameof(usuario.Email), "Informe o e-mail.");
        }

        if (PasswordHelper.ApenasNumeros(usuario.CPF).Length != 11)
        {
            ModelState.AddModelError(nameof(usuario.CPF), "Informe um CPF com 11 números.");
        }

        if (_db.Usuarios.Any(u => u.Email == usuario.Email))
        {
            ModelState.AddModelError(nameof(usuario.Email), "Já existe usuário com este e-mail.");
        }

        if (_db.Usuarios.Any(u => u.CPF == usuario.CPF))
        {
            ModelState.AddModelError(nameof(usuario.CPF), "Já existe usuário com este CPF.");
        }

        if (!ModelState.IsValid)
        {
            return View("Cadastrar", usuario);
        }

        var senhaInicial = PasswordHelper.GerarSenhaInicial(usuario.Nome, usuario.CPF);
        usuario.Senha = PasswordHelper.CriarHash(senhaInicial);
        _db.Usuarios.Add(usuario);
        _db.SaveChanges();

        TempData["Mensagem"] = $"Funcionário cadastrado. Senha inicial: {senhaInicial}";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult RedefinirSenha(int id)
    {
        var login = ExigirLogin();
        if (login is not null) return login;
        if (!IsAdmin) return RedirectToAction("Index", "Home");

        var usuario = _db.Usuarios.Find(id);
        if (usuario is null) return NotFound();

        var senhaInicial = PasswordHelper.GerarSenhaInicial(usuario.Nome, usuario.CPF);
        usuario.Senha = PasswordHelper.CriarHash(senhaInicial);
        _db.SaveChanges();

        TempData["Mensagem"] = $"Senha redefinida para {usuario.Nome}: {senhaInicial}";
        return RedirectToAction("Index");
    }

    private static string FormatCpf(string? cpf)
    {
        var numeros = PasswordHelper.ApenasNumeros(cpf);
        return numeros.Length == 11
            ? $"{numeros[..3]}.{numeros.Substring(3, 3)}.{numeros.Substring(6, 3)}-{numeros[9..]}"
            : cpf ?? string.Empty;
    }
}
