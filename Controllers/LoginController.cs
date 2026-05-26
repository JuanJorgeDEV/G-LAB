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
        ViewBag.EmailLembrado = Request.Cookies["ProjetoOS_Email"];
        return View();
    }

    [HttpPost]
    public IActionResult Entrar(string email, string senha, bool lembrarAcesso = false)
    {
        var usuario = _db.Usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario is null || !PasswordHelper.Verificar(senha, usuario.Senha))
        {
            ViewBag.Erro = "E-mail ou senha invalidos.";
            ViewBag.EmailLembrado = email;
            return View("Index");
        }

        if (!usuario.Senha.StartsWith("PBKDF2$", StringComparison.Ordinal))
        {
            usuario.Senha = PasswordHelper.CriarHash(senha);
            _db.SaveChanges();
        }

        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
        HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil);

        if (lembrarAcesso)
        {
            Response.Cookies.Append("ProjetoOS_Email", usuario.Email, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            });
        }
        else
        {
            Response.Cookies.Delete("ProjetoOS_Email");
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Sair()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
