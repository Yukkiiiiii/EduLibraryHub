using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data.Entities;

namespace EduLibraryHub.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

public DbSet<EduLibraryHub.Data.Entities.Book> Book { get; set; } = default!;

public DbSet<EduLibraryHub.Data.Entities.Review> Review { get; set; } = default!;
}
