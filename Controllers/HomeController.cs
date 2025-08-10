using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace PIM.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return View("LoggedOut");

        var claims = User.Claims
            .Select(c => (Type: c.Type, Value: c.Value))
            .ToList();

        ViewBag.Name   = User.FindFirst("name")?.Value
                         ?? User.Identity?.Name
                         ?? User.FindFirst("given_name")?.Value;
        ViewBag.Email  = User.FindFirst("email")?.Value;
        ViewBag.Pic    = User.FindFirst("urn:google:picture")?.Value;
        ViewBag.Locale = User.FindFirst("urn:google:locale")?.Value;

        return View("Index", claims);
    }
}