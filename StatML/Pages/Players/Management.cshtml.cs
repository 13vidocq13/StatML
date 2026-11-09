using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Players
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public const int PageSize = 10;
        
        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Player> Players { get; set; }
        public List<FarmZone> FarmZones { get; set; }
        public async Task OnGet(int? pageIndex)
        {
            FarmZones = _context.FarmZones.ToList();
            
            int currentPage = pageIndex ?? 1;

            Players = await PaginatedList<Player>.CreateAsync(_context.Players.OrderBy(x => x.PName.ToUpper()), currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostDelete(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.Find(id);

            if (player != null)
            {
                try
                {
                    _context.Players.Remove(player);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Players = await PaginatedList<Player>.CreateAsync(_context.Players.OrderBy(x => x.PName.ToUpper()), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Players/Management");
        }
    }
}
