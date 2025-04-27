using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.Models;
using EduLibraryHub.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IActionResult> Index()
    {
        ViewData["Users"] = await _context.Users.CountAsync();
        ViewData["Books"] = await _context.Books.CountAsync();
        ViewData["Reviews"] = await _context.Reviews.CountAsync();
        ViewData["Tags"] = await _context.Tags.CountAsync();
        ViewData["Genres"] = await _context.Genres.CountAsync();
        ViewData["Borrows"] = await _context.BorrowRecords.CountAsync();
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