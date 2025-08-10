using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace PIM.Controllers;
using System.Security.Claims;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return View("LoggedOut");

        var claims = User.Claims
            .Select(c => (Type: c.Type, Value: c.Value))
            .ToList();

        ViewBag.Name =
            User.FindFirst(ClaimTypes.Name)?.Value
            ?? User.FindFirst("name")?.Value
            ?? User.FindFirst("given_name")?.Value;
        ViewBag.Email =
            User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst("email")?.Value
            ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        ViewBag.Pic    = User.FindFirst("urn:google:picture")?.Value;
        ViewBag.Locale = User.FindFirst("urn:google:locale")?.Value;

        return View("Index", claims);
    }
}