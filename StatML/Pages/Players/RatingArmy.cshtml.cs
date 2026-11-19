using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Players
{
    public class RatingArmyModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EndDate { get; set; }

        private readonly AppDbContext _context;

        public RatingArmyModel(AppDbContext context)
        {
            _context = context;
        }

        public PaginatedList<PlayerRatingDiffViewModel> RatingEntries { get; set; } = default!;

        public async Task OnGetAsync(int? pageIndex)
        {
            DateOnly dateFrom;
            DateOnly dateTo;

            if (string.IsNullOrEmpty(StartDate) || !DateOnly.TryParse(StartDate, out dateFrom))
            {
                dateFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            }

            if (string.IsNullOrEmpty(EndDate) || !DateOnly.TryParse(EndDate, out dateTo))
            {
                dateTo = DateOnly.FromDateTime(DateTime.Today);
            }

            StartDate ??= dateFrom.ToString("yyyy-MM-dd");
            EndDate ??= dateTo.ToString("yyyy-MM-dd");

            var rawData = await _context.RatingArmies
                .Include(r => r.PlayerEntity)
                .Where(r => r.RateDate == dateFrom || r.RateDate == dateTo)
                .ToListAsync();

            var calculatedList = rawData
                .GroupBy(r => r.PlayerId)
                .Select(g => new
                {
                    Group = g,
                    FromRecord = g.FirstOrDefault(x => x.RateDate == dateFrom),
                    ToRecord = g.FirstOrDefault(x => x.RateDate == dateTo)
                })
                .Where(x => x.ToRecord != null)
                .Select(x => new PlayerRatingDiffViewModel
                {
                    PlayerId = x.Group.Key,
                    PlayerName = x.ToRecord?.PlayerEntity?.PName ?? x.FromRecord?.PlayerEntity?.PName ?? "Неизвестно",
                    RateFrom = x.FromRecord?.Rate ?? 0,
                    RateTo = x.ToRecord!.Rate,
                    Difference = x.FromRecord != null ? x.ToRecord!.Rate - x.FromRecord.Rate : 0
                })
                .OrderByDescending(x => x.Difference)
                .ToList();

            int pageSize = 15;
            int pageNumber = pageIndex ?? 1;

            var pageItems = calculatedList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            RatingEntries = new PaginatedList<PlayerRatingDiffViewModel>(
                pageItems,
                calculatedList.Count,
                pageNumber,
                pageSize
            );
        }
    }

    public class PlayerRatingDiffViewModel
    {
        public long PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public long RateFrom { get; set; }
        public long RateTo { get; set; }
        public long Difference { get; set; }
    }
}