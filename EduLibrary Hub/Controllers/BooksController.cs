using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class BooksController : Controller
{
    private readonly LibraryDbContext _dbContext;

    public BooksController(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // 1️⃣ List All Books
    public async Task<IActionResult> Index()
    {
        var books = await _dbContext.Books.ToListAsync();
        return View(books);
    }

    // 2️⃣ Add Book - GET
    public IActionResult Add()
    {
        return View();
    }

    // 2️⃣ Add Book - POST
    [HttpPost]
    public async Task<IActionResult> Add(Book book)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(book);
    }

    // 3️⃣ Edit Book - GET
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null) return NotFound();
        return View(book);
    }

    // 3️⃣ Edit Book - POST
    [HttpPost]
    public async Task<IActionResult> Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
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
