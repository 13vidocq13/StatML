using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Salts
{
    public class CurrentSaltsManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public int PageSize { get; } = 10;
        public PaginatedList<Salt> Salts { get; set; }

        public CurrentSaltsManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            var saltsForPage = new List<Salt>();

           var currentSalts = _context.Salts
                .Include(x => x.PlayerEntity)
                .Include(x => x.CreatorLoginEntity)
                .Include(x => x.EditorLoginEntity)
                .AsEnumerable() // DateOnly → в память
                .Where(x => x.DateSalt.HasValue &&
                            x.DateSalt.Value.ToDateTime(TimeOnly.MinValue)
                                .AddDays(x.DaysForPlayer) >= DateTime.Now)
                .GroupBy(x => x.SaltId) // Группируем по озеру
                .Select(g => g
                    .OrderByDescending(x => x.DateSalt) // самая поздняя дата
                    .First())                           // берём только её
                .OrderByDescending(x => x.DateSalt)     // сортировка по дате (для пагинации)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var item in currentSalts)
            {
                var currentSaltParts = _context.Salts.Where(x => x.SaltId == item.SaltId).Include(x => x.PlayerEntity).Include(x => x.CreatorLoginEntity)
                    .Include(x => x.EditorLoginEntity).OrderByDescending(x => x.DateSalt).ToList();

                foreach (var part in currentSaltParts)
                {
                    if (saltsForPage.Contains(part))
                        continue;

                    saltsForPage.Add(part);
                }
            }

            var countDistinctSalts = saltsForPage.Select(x => x.SaltId).Distinct().Count();

            var saltsCurrentCount = _context.Salts
                .Include(x => x.PlayerEntity)
                .Include(x => x.CreatorLoginEntity)
                .Include(x => x.EditorLoginEntity)
                .AsEnumerable() // DateOnly → в память
                .Where(x => x.DateSalt.HasValue &&
                            x.DateSalt.Value.ToDateTime(TimeOnly.MinValue)
                                .AddDays(x.DaysForPlayer) >= DateTime.Now)
                .GroupBy(x => x.SaltId) 
                .Select(g => g
                    .OrderByDescending(x => x.DateSalt) 
                    .First())                           
                .Count();

            Salts = new PaginatedList<Salt>(saltsForPage, saltsCurrentCount, currentPage, PageSize);
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

            return RedirectToPage("/Salts/CurrentSaltsManagement");
        }
    }
}
