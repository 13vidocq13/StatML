using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Players
{
    public class DetailsModel : PageModel
    {

        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }
        public PaginatedList<Grail> Grails { get; set; }
        public PaginatedList<Salt> Salts { get; set; }
        public PaginatedList<Tournament> Tournaments { get; set; }
        public PaginatedList<FlagTournamentDetails> Flags { get; set; }
        public int GrailsCount { get; set; }
        public int SaltsCount { get; set; }
        public int GrailsBlackGem { get; set; }
        public double SaltsBlackGem { get; set; }
        public int? BasicFalgsCount { get; set; }
        public int? OtherFalgsCount { get; set; }
        public int? NotGvenFalgsCount { get; set; }


        public async Task OnGet(int? id, int? pageIndexGrails, int? pageIndexSalts, int? pageIndexTournaments, int? pageIndexFlags)
        {
            GrailsCount = _context.Grails.Where(x => x.PlayerId == id).Count();
            SaltsCount = _context.Salts.Where(x => x.PlayerId == id).Count();
            GrailsBlackGem = _context.Grails.Where(x => x.PlayerId == id).Sum(x => x.HoursForPlayer) * 200;
            SaltsBlackGem = _context.Salts.Where(x => x.PlayerId == id).Sum(x => x.DaysForPlayer) * 20 * 24;
            BasicFalgsCount = _context.FlagTournamentDetails.Where(x => x.PlayerId == id).Sum(x => x.CountOfBasicFlags);
            OtherFalgsCount = _context.FlagTournamentDetails.Where(x => x.PlayerId == id).Sum(x => x.CountOfOtherFlags);
            NotGvenFalgsCount = _context.FlagTournamentDetails.Where(x => x.PlayerId == id).Sum(x => x.CountOfNotGivenFlags);

            int currentPageGrails = pageIndexGrails ?? 1;
            int currentPageSalts = pageIndexSalts ?? 1;
            int currentPageTournaments = pageIndexTournaments ?? 1;
            int currentPageFlags = pageIndexFlags ?? 1;

            Grails = await PaginatedList<Grail>.CreateAsync(_context.Grails.Include(x => x.PlayerEntity).Where(x => x.PlayerId == id)
                .OrderByDescending(x => x.DateGrail), currentPageGrails, PageSize);
            Salts = await PaginatedList<Salt>.CreateAsync(_context.Salts.Include(x => x.PlayerEntity).Where(x => x.PlayerId == id)
                .OrderByDescending(x => x.DateSalt), currentPageSalts, PageSize);
            Tournaments = await PaginatedList<Tournament>.CreateAsync(_context.Tournaments.Include(x => x.PlayerEntity).Include(x => x.TournamentTypeEntity).Include(x => x.ArtifactEntity)
                .Where(x => x.PlayerId == id).OrderByDescending(x => x.DateOnly), currentPageTournaments, PageSize);

            Flags = await PaginatedList<FlagTournamentDetails>.CreateAsync(_context.FlagTournamentDetails.Include(x => x.PlayerEntity)
                .Where(x => x.PlayerId == id).OrderByDescending(x => x.TournamentDate), currentPageFlags, PageSize);

            Player = _context.Players.Find(id);
        }
    }
}
