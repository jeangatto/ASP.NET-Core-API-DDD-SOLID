using Microsoft.AspNetCore.Mvc;

namespace SGP.PublicApi.Controllers;

[Route("")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => new RedirectResult("~/swagger");
}