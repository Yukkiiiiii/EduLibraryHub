using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Models;          // ��� �� �������� ������ ������ (User, RegisterViewModel, � �.�.)
using EduLibraryHub.database;        // ��� �� ������� ������ LibraryDbContext
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

// ��������� builder-� �� ��� ������������
var builder = WebApplication.CreateBuilder(args);

// ������������� �� ��������� (����������)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ������������ �� DbContext-� � connection string-� �� �������������� (appsettings.json)
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ������������ �� Identity �������� � ����� ������������� ���� User
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;             // ������� ���� ���� �����
    options.Password.RequireLowercase = true;         // ������� ���� ���� ����� �����
    options.Password.RequireUppercase = false;        // �� ������� ������ �����
    options.Password.RequireNonAlphanumeric = false;  // �� ������� ��������� �������
    options.Password.RequiredLength = 6;              // ��������� ������� 6 �������
})
.AddEntityFrameworkStores<LibraryDbContext>()
.AddDefaultTokenProviders();

// ����������� �� Cookie Policy � ��������� �� cookies ���� Secure � SameSite �����������
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
});

// ����������� �� AntiForgery � cookies �� �� Secure
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// ������������ �� ������������ � ������� (MVC)
builder.Services.AddControllersWithViews();

// �������� �� connection string-� �� �������� (����������)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Connection string: {ConnectionString}", connectionString);

var app = builder.Build();

// ��������� ��� HTTPS
app.UseHttpsRedirection();

// ���������� �� �������� ������� � Cookie Policy
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map-���� �� ����������� �������: (��� �� �� �������� � ������������)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
