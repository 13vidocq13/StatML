using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System.Security.Claims;

namespace StatML.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        [BindProperty]
        public Login Login { get; set; }

        public LoginModel(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Login.Email) || string.IsNullOrEmpty(Login.Password))
            {
                ModelState.AddModelError(string.Empty, "Заполните все поля.");
                return Page();
            }

            // Ищем в твоей кастомной таблице Logins
            var existingUser = await _context.Logins
                .Include(x => x.RoleEntity)
                .FirstOrDefaultAsync(l => l.Email.ToLower() == Login.Email.ToLower());

            if (existingUser == null || !_authService.VerifyPassword(Login.Password, existingUser.Password))
            {
                ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.Email),
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Role, existingUser.RoleEntity?.Name ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Создаем куку авторизации
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToPage("/Index");
        }
    }
}