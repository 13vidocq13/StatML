using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System.Linq;

namespace StatML.Pages.Grails
{
    public class GeneralSaltsGrailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<PlayersSaltsGrailsData> Players { get; set; }

        public GeneralSaltsGrailsModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            var saltGrailsData = _context.Players
                .Include(x => x.SaltEntities)
                .Include(x => x.GrailEntities)
                .Select(p => new
                {
                    Player = p,
                    TotalBlackGem = p.SaltEntities.Sum(x => x.DaysForPlayer * 20 * 24) + p.GrailEntities.Sum(x => x.HoursForPlayer * 200),
                    SaltsDays = p.SaltEntities.Sum(x => x.DaysForPlayer),
                    SaltsGem = p.SaltEntities.Sum(x => x.DaysForPlayer * 20 * 24),
                    GrailsHours = p.GrailEntities.Sum(x => x.HoursForPlayer),
                    GrailsGem = p.GrailEntities.Sum(x => x.HoursForPlayer * 200)
                })
                .OrderByDescending(x => x.TotalBlackGem)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var players = new List<PlayersSaltsGrailsData>();

            foreach (var item in saltGrailsData)
            {
                players.Add(new PlayersSaltsGrailsData
                {
                    Player = item.Player,
                    TotalBlackGem = item.TotalBlackGem,
                    SaltsDays = item.SaltsDays,
                    SaltsGem = item.SaltsGem,
                    GrailsHours = item.GrailsHours,
                    GrailsGem = item.GrailsGem
                });
            }

            Players = new PaginatedList<PlayersSaltsGrailsData>(players, _context.Players.Count(), currentPage, PageSize);
        }
    }

    public class PlayersSaltsGrailsData
    {
        public Player Player { get; set; }
        public double TotalBlackGem { get; set; }
        public double SaltsDays { get; set; }
        public double SaltsGem { get; set; }
        public int GrailsHours { get; set; }
        public int GrailsGem { get; set; }

    }
}
