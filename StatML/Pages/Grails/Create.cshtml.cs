using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Grails
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Grail Grail { get; set; }
        public SelectList Players { get; set; }
        public IActionResult OnGet()
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

            Grail.CreatorId = _context.Logins.FirstOrDefault(x => x.Email == Request.Cookies["UserEmail"].ToLower()).Id;

            _context.Grails.Add(Grail);
            _context.SaveChanges();

            return RedirectToPage("/Grails/Management");
        }
    }
}
