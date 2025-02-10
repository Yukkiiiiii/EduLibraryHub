using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")] // Restrict access to Admins only
public class AdminController : Controller
{
    private readonly LibraryDbContext _dbContext;

    public AdminController(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Admin Dashboard
    public IActionResult Dashboard()
    {
        return View();
    }

    // List all users
    public async Task<IActionResult> Users()
    {
        var users = await _dbContext.Users.ToListAsync();
        return View(users);
    }

    // Edit a user
    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(User user)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Users");
        }
        return View(user);
    }

    // Delete a user
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Users");
    }

    // View System Logs (Basic Example)
    public IActionResult Logs()
    {
        // Dummy logs for now
        var logs = new[]
        {
            new { Time = "2025-02-07 10:00 AM", Action = "User John registered" },
            new { Time = "2025-02-07 10:15 AM", Action = "Admin deleted a user" }
        };
        return View(logs);
    }
}
