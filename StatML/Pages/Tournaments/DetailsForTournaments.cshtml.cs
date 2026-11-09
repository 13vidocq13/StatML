using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System.Drawing.Printing;

namespace StatML.Pages.Tournaments
{
    public class DetailsForTournamentsModel : PageModel
    {
        private readonly AppDbContext _context;
        public const int PageSize = 10;
        public PaginatedList<DateOnly> TournamentsDates { get; set; }
        
        public void OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            var tournamentsDates = _context.FlagTournamentDetails
                .OrderByDescending(x => x.TournamentDate).GroupBy(x => x.TournamentDate).Select(x => x.Key).ToList();

            TournamentsDates = new PaginatedList<DateOnly>(tournamentsDates, tournamentsDates.Count, currentPage, PageSize);
        }
    }
}
