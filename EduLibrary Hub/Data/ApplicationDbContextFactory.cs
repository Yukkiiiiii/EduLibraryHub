using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EduLibraryHub.database;

namespace EduLibraryHub.database
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EduLibraryHubDb;Trusted_Connection=True;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}