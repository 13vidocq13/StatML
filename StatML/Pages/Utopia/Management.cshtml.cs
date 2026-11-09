using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Utopia
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Models.Utopia> Utopias { get; set; }
        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Utopias = await PaginatedList<Models.Utopia>.CreateAsync(_context.Utopias.Include(x => x.PlayerEntity1).Include(x => x.PlayerEntity2)
                        .OrderByDescending(x => x.DateOnly), currentPage, PageSize);

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var utopia = _context.Utopias.Find(id);

            if (utopia != null)
            {
                try
                {
                    _context.Utopias.Remove(utopia);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);

                    Utopias = await PaginatedList<Models.Utopia>.CreateAsync(_context.Utopias.Include(x => x.PlayerEntity1).Include(x => x.PlayerEntity2)
                        .OrderByDescending(x => x.DateOnly), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Utopia/Management");
        }
    }
}
