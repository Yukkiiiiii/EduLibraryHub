using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Models;          // Тук се съдържат вашите модели (User, RegisterViewModel, и т.н.)
using EduLibraryHub.database;        // Тук се съдържа вашият LibraryDbContext
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

// Създаваме builder-а за уеб приложението
var builder = WebApplication.CreateBuilder(args);

// Конфигуриране на логването (опционално)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Регистриране на DbContext-а с connection string-а от конфигурацията (appsettings.json)
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Регистриране на Identity услугите с вашия потребителски клас User
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;             // Изисква поне една цифра
    options.Password.RequireLowercase = true;         // Изисква поне една малка буква
    options.Password.RequireUppercase = false;        // Не изисква главна буква
    options.Password.RequireNonAlphanumeric = false;  // Не изисква специални символи
    options.Password.RequiredLength = 6;              // Минимална дължина 6 символа
})
.AddEntityFrameworkStores<LibraryDbContext>()
.AddDefaultTokenProviders();

// Настройване на Cookie Policy – маркиране на cookies като Secure и SameSite ограничение
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
});

// Настройване на AntiForgery – cookies да са Secure
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Регистриране на контролерите с изгледи (MVC)
builder.Services.AddControllersWithViews();

// Логиране на connection string-а за проверка (опционално)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Connection string: {ConnectionString}", connectionString);

var app = builder.Build();

// Насочване към HTTPS
app.UseHttpsRedirection();

// Използване на статични файлове и Cookie Policy
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map-ване на стандартния маршрут: (ако не са зададени в контролерите)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
