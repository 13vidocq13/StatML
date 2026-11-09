using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Salts
{
    public class ManagementArchiveModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Salt> Salts { get; set; }
        
        public ManagementArchiveModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Salts = await PaginatedList<Salt>.CreateAsync(_context.Salts.Include(x => x.PlayerEntity).Include(x => x.CreatorLoginEntity)
                .Include(x => x.EditorLoginEntity).OrderByDescending(x => x.DateSalt), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var salt = _context.Salts.Find(id);

            if (salt != null)
            {
                try
                {
                    _context.Salts.Remove(salt);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Salts = await PaginatedList<Salt>.CreateAsync(_context.Salts.Include(x => x.PlayerEntity).OrderByDescending(x => x.DateSalt), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Salts/Management");
        }
    }
}
