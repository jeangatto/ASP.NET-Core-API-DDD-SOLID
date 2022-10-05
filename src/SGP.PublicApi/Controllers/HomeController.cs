using Microsoft.AspNetCore.Mvc;

namespace SGP.PublicApi.Controllers;

[Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => new RedirectResult("~/swagger");
}