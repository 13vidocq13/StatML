using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Tournaments
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public const int PageSize = 12;
        public PaginatedList<Tournament> Tournaments { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Tournaments = await PaginatedList<Tournament>.CreateAsync(_context.Tournaments.Include(x => x.PlayerEntity).Include(x => x.TournamentTypeEntity).Include(x => x.ArtifactEntity).
                Include(x => x.ArtifactEntity.SetEntity).
                OrderByDescending(x => x.DateOnly).ThenBy(x => x.PositionInTournament), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var tournament = _context.Tournaments.Find(id);

            if (tournament != null)
            {
                try
                {
                    _context.Tournaments.Remove(tournament);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Tournaments = await PaginatedList<Tournament>.CreateAsync(_context.Tournaments.Include(x => x.PlayerEntity).Include(x => x.TournamentTypeEntity).Include(x => x.ArtifactEntity).
                        Include(x => x.ArtifactEntity.SetEntity).
                        OrderByDescending(x => x.DateOnly).ThenBy(x => x.PositionInTournament), currentPage, PageSize);
                }
            }

            return RedirectToPage("/Tournaments/Management");
        }
    }
}
