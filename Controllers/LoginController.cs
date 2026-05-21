using Microsoft.AspNetCore.Mvc;
using ProjetoOS.Models;

namespace ProjetoOS.Controllers;

public class LoginController : Controller
{
    private readonly AppDbContext _db;

    public LoginController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Entrar(string email, string senha)
    {
        var usuario = _db.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);

        if (usuario is null)
        {
            ViewBag.Erro = "E-mail ou senha invalidos.";
            return View("Index");
        }

        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
        HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Sair()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
