using Microsoft.AspNetCore.Mvc;

namespace AutoFusion.Web.Controllers;

public class ReportingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
