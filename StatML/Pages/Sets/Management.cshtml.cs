using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Sets
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Sett> Sets { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Sets = await PaginatedList<Sett>.CreateAsync(_context.Sets.OrderByDescending(x => x.Level).ThenBy(x => x.Id), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var sett = _context.Sets.Find(id);

            if (sett != null)
            {
                try
                {
                    _context.Sets.Remove(sett);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Sets = await PaginatedList<Sett>.CreateAsync(_context.Sets.OrderByDescending(x => x.Level).ThenBy(x => x.Id), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Sets/Management");
        }
    }
}
