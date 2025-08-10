using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
namespace PIM.Controllers;
public class AuthController : Controller
{
    // Страница выбора провайдера
    [HttpGet("auth/login")]
    public IActionResult Login(string? returnUrl = "/")
    {
        ViewBag.ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
        return View("Login");
    }

    // Вызов внешнего провайдера по имени (с белым списком)
    [HttpGet("auth/external/{provider}")]
    public IActionResult ExternalLogin([FromRoute] string provider, string? returnUrl = "/")
    {
        // Разрешаем только эти схемы
        var allowed = new[] { "Google", "GitHub" };
        if (!allowed.Contains(provider, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Unknown auth provider");

        var props = new AuthenticationProperties { RedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl };
        return Challenge(props, provider);
    }

    [HttpPost("auth/logout")]
    public IActionResult Logout()
        => SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme);
}