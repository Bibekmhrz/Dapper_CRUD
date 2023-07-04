using Microsoft.AspNetCore.Mvc;

namespace Dapper_CRUD.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
