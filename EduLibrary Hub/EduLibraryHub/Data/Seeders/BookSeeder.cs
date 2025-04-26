using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EduLibraryHub.Data;
using EduLibraryHub.Models;
using EduLibraryHub.Data.Entities;
using Microsoft.VisualBasic.FileIO;

namespace EduLibraryHub.Data.Seeders
{
    public static class LibrarySeeder
    {
        public static async Task Seed(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            ctx.Database.Migrate();
            if (!ctx.Books.Any())
            {
                var path = Path.Combine(env.ContentRootPath, "Data", "BookData.csv");
                var lines = File.ReadAllLines(path, Encoding.GetEncoding("windows-1251")).Skip(1);

                using var parser = new TextFieldParser(path, Encoding.GetEncoding("windows-1251"));
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                parser.ReadLine();

                var items = lines.Select(line =>
                {
                    var parts = parser.ReadFields();
                    return new Book
                    {
                        //Id = int.Parse(parts[0]),
                        Title = parts[1].Trim('"'),
                        Author = parts[2].Trim('"'),
                        ReleaseYear = parts[3].Trim('"'),
                        Tome = parts[4].Trim('"'),
                        Inventory = parts[5].Trim('"')
                    };
                }).ToList();
                ctx.Books.AddRange(items);
                ctx.SaveChanges();
            }
        }
    }
}
