using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.Utopia
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        public SelectList Players { get; set; }
        [BindProperty]
        public Models.Utopia Utopia { get; set; }

        public IActionResult OnGet(int? Id)
        {
            var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToUpper()).ToList();

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

            Utopia = _context.Utopias.Find(Id);

            if (Players == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToUpper()).ToList();

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

            _context.Utopias.Update(Utopia);
            _context.SaveChanges();

            return RedirectToPage("/Utopia/Management");
        }
    }
}
