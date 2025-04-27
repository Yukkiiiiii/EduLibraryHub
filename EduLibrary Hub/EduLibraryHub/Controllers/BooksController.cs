using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data;
using EduLibraryHub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduLibraryHub.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 20;

        public BooksController(ApplicationDbContext context)
            => _context = context;

        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(
                await _context.Books.CountAsync()
                / (double)PageSize
            );

            var books = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .Include(b => b.Reviews)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var query = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .Include(b => b.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchString) ||
                    b.Author.Contains(searchString)
                );
            }

            var rows = await query
                .Take(PageSize)               
                .ToListAsync();

            return PartialView("_BookRows", rows);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .Include(b => b.Reviews).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            return View(book);
        }

        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Data.Entities.Book book, int[] TagIds)
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
            var book = await _context.Books.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", book.Tags.Select(t => t.Id));
            return View(book);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Data.Entities.Book book, int[] TagIds)
        {
            if (id != book.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
                ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", TagIds);
                return View(book);
            }

            var existing = await _context.Books.Include(b => b.Tags).FirstAsync(b => b.Id == id);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Books.Include(b => b.Genre).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null) _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
