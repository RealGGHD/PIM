using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
namespace PIM.Controllers;
public class AuthController : Controller
{
    [HttpGet("auth/login")]
    public IActionResult Login(string? returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = returnUrl ?? "/" };
        return Challenge(props, "Google");
    }

    [HttpPost("auth/logout")]
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}