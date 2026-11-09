using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System;
using System.Security.Claims;

namespace StatML.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        [BindProperty]
        public Login Login { get; set; }
       
        public RegisterModel(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Login.Email) || string.IsNullOrEmpty(Login.Password))
            {
                ModelState.AddModelError(string.Empty, "Заполните все поля.");
                return Page();
            }

            var existingUser = await _context.Logins.FirstOrDefaultAsync(l => l.Email.ToLower() == Login.Email.ToLower());
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким email уже существует.");
                return Page();
            }

            // Хешируем пароль
            var passwordHash = _authService.HashPassword(Login.Password);
            var login = new Login
            {
                Email = Login.Email.ToLower(),
                Password = passwordHash,
                RoleId = 5, // Роль "User" по умолчанию 
                PlayerId = 27
            };

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            // Сохраняем email после регистрации
            //Response.Cookies.Append("UserEmail", login.Email, new CookieOptions { HttpOnly = false, Expires = DateTime.Now.AddHours(1) });
            //Response.Cookies.Append("AuthToken", Guid.NewGuid().ToString(), new CookieOptions { HttpOnly = true, Expires = DateTime.Now.AddHours(1) });

            existingUser = await _context.Logins
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

            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme); // Используем Identity.Application
            var principal = new ClaimsPrincipal(identity);

            try
            {
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });
                Console.WriteLine("Signed in successfully");

                var setCookieHeader = HttpContext.Response.Headers["Set-Cookie"];
                if (setCookieHeader.Any())
                {
                    Console.WriteLine("Set-Cookie headers:");
                    foreach (var header in setCookieHeader)
                    {
                        Console.WriteLine($"  {header}");
                    }
                }
                else
                {
                    Console.WriteLine("No Set-Cookie headers found.");
                }

                Console.WriteLine($"User.Identity.IsAuthenticated: {User.Identity.IsAuthenticated}"); // Отладка
                await Task.Delay(100); // Задержка для отладки
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignIn failed: {ex.Message}");
            }

            var token = Guid.NewGuid().ToString();
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                SameSite = SameSiteMode.Lax
            });

            Response.Cookies.Append("UserEmail", Login.Email, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                SameSite = SameSiteMode.Lax
            });

            Response.Cookies.Append("UserRole", existingUser.RoleEntity?.Name ?? "User", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(1),
                SameSite = SameSiteMode.Lax
            });

            return RedirectToPage("/Index");
        }
    }
}
