using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.Models;
using EduLibraryHub.Data;

namespace EduLibraryHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["Books"] = _context.Books.Count();
        ViewData["Reviews"] = _context.Reviews.Count();
        ViewData["Users"] = _context.Users.Count();
        ViewData["Tags"] = _context.Tags.Count();
        ViewData["Genres"] = _context.Genres.Count();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}