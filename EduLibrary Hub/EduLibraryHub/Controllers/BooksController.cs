using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data;
using EduLibraryHub.Data.Entities;

namespace EduLibraryHub.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .Include(b => b.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null) return NotFound();

            return View(book);
        }

        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Book book,
            int[] TagIds)
        {
            if (ModelState.IsValid)
            {
                if (TagIds != null)
                {
                    foreach (var tagId in TagIds)
                    {
                        var tag = await _context.Tags.FindAsync(tagId);
                        book.Tags.Add(tag);
                    }
                }
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", TagIds);
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", book.Tags.Select(t => t.Id));
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Book book,
            int[] TagIds)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Books
                    .Include(b => b.Tags)
                    .FirstAsync(b => b.Id == id);

                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ReleaseYear = book.ReleaseYear;
                existing.Tome = book.Tome;
                existing.Inventory = book.Inventory;
                existing.GenreId = book.GenreId;

                existing.Tags.Clear();
                if (TagIds != null)
                {
                    foreach (var tagId in TagIds)
                    {
                        var tag = await _context.Tags.FindAsync(tagId);
                        existing.Tags.Add(tag);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", TagIds);
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null) _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
