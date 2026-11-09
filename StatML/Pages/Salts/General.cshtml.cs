using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Salts
{
    public class GeneralModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Player> Players { get; set; }

        public GeneralModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Players = await PaginatedList<Player>.CreateAsync(_context.Players.Include(x => x.SaltEntities).OrderByDescending(x => x.SaltEntities.Sum(x => x.DaysForPlayer)),
                currentPage, PageSize);
        }
    }
}
