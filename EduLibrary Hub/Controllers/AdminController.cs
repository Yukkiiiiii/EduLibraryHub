using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.database;
using EduLibraryHub.Models;

namespace EduLibraryHub.Controllers
{
public class AdminController : Controller
{
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
    {
            _context = context;
    }

    // Admin Dashboard
    public IActionResult Dashboard()
    {
            var books = _context.Books.ToList();
            var users = _context.Users.ToList();
            return View(new AdminDashboardViewModel { Books = books, Users = users });
    }

    // List all users
    public async Task<IActionResult> Users()
    {
        var users = await _dbContext.Users.ToListAsync();
        return View(users);
    }

    public class AdminDashboardViewModel
    {
        public List<Book> Books { get; set; }
        public List<User> Users { get; set; }
    }
}
