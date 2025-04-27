using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
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
        private const int PageSize = 20;

        public BooksController(ApplicationDbContext context)
            => _context = context;

        public async Task<IActionResult> Index(string searchString, int? genreId, int[] tagIds, int page = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenre"] = genreId;
            ViewData["CurrentTags"] = tagIds;
            ViewData["Genres"] = new SelectList(
                await _context.Genres.OrderBy(g => g.Name).ToListAsync(),
                "Id", "Name", genreId);
            ViewData["Tags"] = new MultiSelectList(
                await _context.Tags.OrderBy(t => t.Name).ToListAsync(),
                "Id", "Name", tagIds);

            var query = _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Tags)
                .Include(b => b.Reviews)
                .Include(b => b.BorrowRecords)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(b =>
                    b.Title.Contains(searchString) ||
                    b.Author.Contains(searchString));

            if (genreId.HasValue)
                query = query.Where(b => b.GenreId == genreId.Value);

            if (tagIds?.Length > 0)
                query = query.Where(b =>
                    b.Tags.Any(t => tagIds.Contains(t.Id)));

            var totalCount = await query.CountAsync();
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            var books = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            foreach (var b in books)
            {
                if (int.TryParse(b.Inventory, out var inv))
                {
                    var activeLoans = b.BorrowRecords.Count(br => br.ReturnDate == null);
                    b.Inventory = (inv - activeLoans).ToString();
                }
                else
                {
                    b.Inventory = "0";
                }
            }

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
                query = query.Where(b =>
                    b.Title.Contains(searchString) ||
                    b.Author.Contains(searchString));

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
                .Include(b => b.BorrowRecords)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activeBorrow = book.BorrowRecords
                .FirstOrDefault(br => br.UserId == userId && br.ReturnDate == null);

            if (int.TryParse(book.Inventory, out var inv))
            {
                var avail = inv - book.BorrowRecords.Count(br => br.ReturnDate == null);
                ViewBag.Available = avail;
            }
            else ViewBag.Available = 0;

            ViewBag.ActiveBorrow = activeBorrow;
            return View(book);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books
                .Include(b => b.BorrowRecords)
                .FirstOrDefaultAsync(b => b.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (book != null && int.TryParse(book.Inventory, out var inv))
            {
                var available = inv - book.BorrowRecords.Count(br => br.ReturnDate == null);
                if (available > 0)
                {
                    _context.BorrowRecords.Add(new BorrowRecord
                    {
                        BookId = id,
                        UserId = userId,
                        BorrowDate = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var record = await _context.BorrowRecords
                .Where(br => br.BookId == id && br.UserId == userId && br.ReturnDate == null)
                .FirstOrDefaultAsync();

            if (record != null)
            {
                record.ReturnDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Book book, int[] TagIds)
        {
            if (ModelState.IsValid)
            {
                if (TagIds != null)
                    foreach (var tagId in TagIds)
                        book.Tags.Add(await _context.Tags.FindAsync(tagId));

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ReleaseYear,Tome,Inventory,GenreId")] Book book, int[] TagIds)
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
                foreach (var tagId in TagIds)
                    existing.Tags.Add(await _context.Tags.FindAsync(tagId));

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
