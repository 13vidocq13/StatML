using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Grails
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Grail> Grails { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;
            
            Grails = await PaginatedList<Grail>.CreateAsync(_context.Grails.Include(x => x.PlayerEntity).Include(x => x.CreatorLoginEntity)
                .Include(x => x.EditorLoginEntity).OrderByDescending(x => x.DateGrail), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var grail = _context.Grails.Find(id);

            if (grail != null)
            {
                try
                {
                    _context.Grails.Remove(grail);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                   
                    Grails = await PaginatedList<Grail>.CreateAsync(_context.Grails.Include(x => x.PlayerEntity).Include(x => x.CreatorLoginEntity)
                        .Include(x => x.EditorLoginEntity).OrderByDescending(x => x.DateGrail), currentPage, PageSize);
                }
            }

            return RedirectToPage("/Grails/Management");
        }
    }
}
