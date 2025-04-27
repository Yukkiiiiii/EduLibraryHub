using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data;
using EduLibraryHub.Data.Entities;

namespace EduLibraryHub.Controllers
{
    public class BorrowRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordsController(ApplicationDbContext context)
            => _context = context;

        public async Task<IActionResult> Index()
        {
            var records = await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
            return View(records);
        }
    }
}
