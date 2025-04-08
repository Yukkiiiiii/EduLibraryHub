using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.database;
using EduLibraryHub.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EduLibraryHub.Controllers
{
public class BooksController : Controller
{
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
    {
            _context = context;
    }

        // Списък с книги
        public IActionResult Index()
    {
            var books = _context.Books.ToList();
        return View(books);
    }

        // Показване на формата за добавяне на нова книга
        [HttpGet]
        public IActionResult Create()
    {
        return View();
    }

        // Обработка на данните от формата за добавяне
    [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
                _context.Books.Add(book);
                _context.SaveChanges();
            return RedirectToAction("Index");
        }
            // Ако ModelState не е валиден, връщаме формата с грешки
        return View(book);
    }

    // 4️⃣ Delete Book
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null) return NotFound();

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
