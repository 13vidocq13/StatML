using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System.Threading.Tasks;

namespace StatML.Pages.Salts
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Salt Salt { get; set; }
        public SelectList Players { get; set; }
        public IActionResult OnGet(int? saltId)
        {
            if(saltId != null)
                Salt = _context.Salts.Find(saltId);
            
            var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToLower()).ToList();

            int separatorIndex = players.FindIndex(x => x.IsGeneral == false);

            if (separatorIndex > 0 && separatorIndex < players.Count)
            {
                players.Insert(separatorIndex, new Player
                {
                    Id = -1,
                    PName = "-- Не в распределении -- ", // Текст разделителя

                });
            }

            Players = new SelectList(players, "Id", "PName");

            return Page();
        }

        public async Task<IActionResult> OnPost(int? saltId)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    var fieldName = modelState.Key; // Название поля
                    var errors = modelState.Value.Errors; // Список ошибок для этого поля
                    foreach (var error in errors)
                    {
                        // Выводим сообщение об ошибке
                        Console.WriteLine($"Поле: {fieldName}, Ошибка: {error.ErrorMessage}");
                    }
                }

                if (saltId != null)
                    Salt = _context.Salts.Find(saltId);

                var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToLower()).ToList();

                int separatorIndex = players.FindIndex(x => x.IsGeneral == false);

                if (separatorIndex > 0 && separatorIndex < players.Count)
                {
                    players.Insert(separatorIndex, new Player
                    {
                        Id = -1,
                        PName = "-- Не в распределении -- ", // Текст разделителя

                    });
                }

                Players = new SelectList(players, "Id", "PName");
                return Page();
            }

            if(Request.Cookies["UserEmail"] == null)
                return RedirectToPage("/Login");

            Salt.CreatorId = _context.Logins.FirstOrDefault(x => x.Email == Request.Cookies["UserEmail"].ToLower()).Id;
            Salt.SaltId = saltId;
            _context.Salts.Add(Salt);
            _context.SaveChanges();

            if(saltId == null)
            {
                Salt.SaltId = Salt.Id;
                _context.Salts.Update(Salt);
                _context.SaveChanges();
            }

            return RedirectToPage("/Salts/CurrentSaltsManagement");
        }
    }
}
