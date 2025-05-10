using Microsoft.AspNetCore.Mvc;

namespace Stefanini.Api.Controllers.v2;

public class ClientController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}