using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.database;
using EduLibraryHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Използвайте модел от отделен файл (например, Models/AdminDashboardViewModel), или премахнете вътрешната дефиниция
            return View(new AdminDashboardViewModel { Books = books, Users = users });
        }

        // List all users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
