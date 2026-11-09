using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Artifacts
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 12;
        public PaginatedList<Artifact> Artifacts { get; set; }
        public Sett SetEntity { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Artifacts = await PaginatedList<Artifact>.CreateAsync(_context.Artifacts.Include(x => x.SetEntity).OrderByDescending(x => x.SetEntity.Level)
                        .ThenBy(x => x.SetId).ThenBy(x => x.Id), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var artifact = _context.Artifacts.Find(id);

            if (artifact != null)
            {
                try
                {
                    _context.Artifacts.Remove(artifact);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Artifacts = await PaginatedList<Artifact>.CreateAsync(_context.Artifacts.Include(x => x.SetEntity).OrderByDescending(x => x.SetEntity.Level)
                        .ThenBy(x => x.SetId).ThenBy(x => x.Id), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Sets/Management");
        }
    }
}
