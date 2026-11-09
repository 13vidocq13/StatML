using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Sets
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Sett> Sets { get; set; }

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Sets = await PaginatedList<Sett>.CreateAsync(_context.Sets.OrderByDescending(x => x.Level).OrderByDescending(x => x.Id),
                currentPage, PageSize);
        }
    }
}
