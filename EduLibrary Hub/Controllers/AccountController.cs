using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EduLibraryHub.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EduLibraryHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        // Конструктор: DI контейнерът доставя UserManager и SignInManager
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Невалиден опит за вход.");
            }
            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ако ModelState е невалиден, ще видите грешките във View чрез ValidationSummary.
                return View(model);
            }

            // Създаваме нов потребител с попълнените данни
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Ако регистрацията е успешна, записваме съобщение и пренасочваме към Login
                TempData["SuccessMessage"] = "Регистрацията е успешна. Моля, влезте във Вашия акаунт.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Ако има грешки от Identity (напр. паролата не отговаря на политиката), ги добавяме към ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}