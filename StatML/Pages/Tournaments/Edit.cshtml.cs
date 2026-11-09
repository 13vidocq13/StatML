using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Tournaments
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tournament Tournament { get; set; }
        public SelectList TypesOfTournaments { get; set; }
        public SelectList Players { get; set; }
        public SelectList Artifacts { get; set; }
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToUpper()).ToList();

            int separatorIndex = players.FindIndex(x => x.IsGeneral == false);

            if (separatorIndex > 0 && separatorIndex < players.Count)
            {
                players.Insert(separatorIndex, new Player
                {
                    Id = -1,
                    PName = "-- Не в распределении -- " // Текст разделителя
                });
            }

            Players = new SelectList(players, "Id", "PName");

            Tournament = _context.Tournaments.Find(id);

            TypesOfTournaments = new SelectList(_context.TournamentTypes.ToList(), "Id", "TypeName");

            var artifactsWithSets = _context.Artifacts.Include(a => a.SetEntity).OrderByDescending(x => x.SetEntity.Level).ThenBy(x => x.SetEntity.Id)
                                        .Select(a => new
                                        {
                                            Id = a.Id,
                                            DisplayText = $"{a.Name} ({a.SetEntity.Name}[{a.SetEntity.Level}])"
                                        }).ToList();
            Artifacts = new SelectList(artifactsWithSets, "Id", "DisplayText");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Players = new SelectList(_context.Players.OrderBy(x => x.PName.ToUpper()).ToList()
                         , "Id", "PName");

                TypesOfTournaments = new SelectList(_context.TournamentTypes.ToList(), "Id", "Name");

                var artifactsWithSets = _context.Artifacts.Include(a => a.SetEntity).OrderByDescending(x => x.SetEntity.Level).ThenBy(x => x.SetEntity.Id)
                                        .Select(a => new
                                        {
                                            Id = a.Id,
                                            DisplayText = $"{a.Name} ({a.SetEntity.Name}[{a.SetEntity.Level}])"
                                        }).ToList();
                Artifacts = new SelectList(artifactsWithSets, "Id", "DisplayText");
                return Page();
            }

            _context.Tournaments.Update(Tournament);
            _context.SaveChanges();

            return RedirectToPage("/Tournaments/Management");
        }
    }
}
