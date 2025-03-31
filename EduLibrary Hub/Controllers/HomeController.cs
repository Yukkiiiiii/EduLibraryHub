using Microsoft.AspNetCore.Mvc;

namespace EduLibraryHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
