using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Promotions
{
    public class ManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Promotion> Promotions { get; set; }
        public IList<DaysFromLastPromotion> DaysFromLastPromotions { get; set; }

        public ManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Promotions = await PaginatedList<Promotion>.CreateAsync(_context.Promotions.OrderByDescending(x => x.DateOnly), currentPage, PageSize);

            DaysFromLastPromotions = new List<DaysFromLastPromotion>();

            foreach (var item in Promotions.Items)
            {
                DateOnly? previousDate = null;

                if (item.Level != 0)
                    previousDate = _context.Promotions.Where(x => x.DateOnly < item.DateOnly && x.DateOnly != null && x.Level > 0).OrderByDescending(x => x.DateOnly)
                        .Select(x => x.DateOnly).FirstOrDefault();
                else
                    previousDate = _context.Promotions.Where(x => x.DateOnly < item.DateOnly && x.DateOnly != null && x.Level == 0).OrderByDescending(x => x.DateOnly)
                        .Select(x => x.DateOnly).FirstOrDefault();

                DaysFromLastPromotions.Add(new DaysFromLastPromotion
                {
                    PromotionId = item.Id,
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

            var promotion = _context.Promotions.Find(id);

            if (promotion != null)
            {
                try
                {
                    _context.Promotions.Remove(promotion);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    Promotions = await PaginatedList<Promotion>.CreateAsync(_context.Promotions.OrderByDescending(x => x.DateOnly), currentPage, PageSize);
                    return Page();
                }
            }

            return RedirectToPage("/Promotions/Management");
        }
    }

    public class DaysFromLastPromotion
    {
        public int PromotionId { get; set; }
        public int? Days { get; set; }
    }
}
