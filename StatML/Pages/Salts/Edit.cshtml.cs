using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.Salts
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Salt Salt { get; set; }
        public SelectList Players { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Salt = _context.Salts.Find(id);

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

            if (Salt == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
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

            if (Request.Cookies["UserEmail"] == null)
                return RedirectToPage("/Login");

            Salt.EditorId = _context.Logins.FirstOrDefault(x => x.Email == Request.Cookies["UserEmail"].ToLower()).Id;

            _context.Salts.Update(Salt);
            _context.SaveChanges();

            return RedirectToPage("/Salts/CurrentSaltsManagement");
        }
    }
}
