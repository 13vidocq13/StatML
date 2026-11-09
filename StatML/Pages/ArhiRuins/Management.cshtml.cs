using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;
using StatML.Pages.Promotions;

namespace StatML.Pages.ArhiRuins
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<ArhiRuin> ArhiRuins { get; set; }
        public IList<DaysFromLastArhiRuins>? DaysFromLastArhiRuin { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            var aaa = _context.ArhiRuins
                        .Select(x => x.DateOnly)
                        .Distinct()
                        .OrderByDescending(x => x)
                        .Skip((currentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();


            ArhiRuins = await PaginatedList<ArhiRuin>.CreateAsync(_context.ArhiRuins.Include(x => x.PlayerEntity).OrderByDescending(x => x.DateOnly), currentPage, PageSize);

            DaysFromLastArhiRuin = new List<DaysFromLastArhiRuins>();

            foreach (var item in ArhiRuins.Items)
            {
                DateOnly? previousDate = null;

                previousDate = _context.ArhiRuins.Where(x => x.DateOnly < item.DateOnly && x.DateOnly != null).OrderByDescending(x => x.DateOnly)
                        .Select(x => x.DateOnly).FirstOrDefault();

                DaysFromLastArhiRuin.Add(new DaysFromLastArhiRuins
                {
                    ArhiRuinId = item.Id,
                    Days = previousDate != DateOnly.MinValue ?
                        item.DateOnly.DayNumber - previousDate.Value.DayNumber : 0
                });
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var arhiRuin = _context.ArhiRuins.Find(id);

            if (arhiRuin != null)
            {
                try
                {
                    _context.ArhiRuins.Remove(arhiRuin);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    ArhiRuins = await PaginatedList<ArhiRuin>.CreateAsync(_context.ArhiRuins.Include(x => x.PlayerEntity).OrderByDescending(x => x.DateOnly), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/ArhiRuins/Management");
        }
    }

    public class DaysFromLastArhiRuins
    {
        public int ArhiRuinId { get; set; }
        public int? Days { get; set; }
    }
}
