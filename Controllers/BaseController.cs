using Microsoft.AspNetCore.Mvc;

namespace ProjetoOS.Controllers;

public abstract class BaseController : Controller
{
    protected int? UsuarioId => HttpContext.Session.GetInt32("UsuarioId");
    protected string UsuarioNome => HttpContext.Session.GetString("UsuarioNome") ?? string.Empty;
    protected string UsuarioPerfil => HttpContext.Session.GetString("UsuarioPerfil") ?? string.Empty;
    protected bool IsAdmin => UsuarioPerfil == "Administrador";

    protected IActionResult? ExigirLogin()
    {
        return UsuarioId is null ? RedirectToAction("Index", "Login") : null;
    }
}
