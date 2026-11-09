using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using StatML.Models;
using System.Threading.Tasks;

namespace StatML.Pages.Tournaments
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public const int PageSize = 10;
        public string Message { get; set; }
        public List<List<string>> ExcelData { get; set; }
        public PaginatedList<Player> Players { get; set; }
        public DateOnly? tournamentDate { get; set; }

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int? pageIndex, DateOnly? date)
        {
            int currentPage = pageIndex ?? 1;

            var query = _context.Players.Include(x => x.FlagTournamentDetailsEntities).OrderByDescending(p => p.FlagTournamentDetailsEntities.Sum(d => d.CountOfOtherFlags))
                .ThenBy(p => p.FlagTournamentDetailsEntities.Sum(d => d.CountOfBasicFlags));

            //if(date != null)
            //    query.

            Players = await PaginatedList<Player>.CreateAsync(query, currentPage, PageSize);
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Message = "Пожалуйста, выберите файл.";
                return Page();
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                Message = "Файл должен быть в формате .xlsx";
                return Page();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        ExcelData = new List<List<string>>();
                        int totalRows = worksheet.Dimension.End.Row;
                        int totalColumns = worksheet.Dimension.End.Column;

                        for (int row = 1; row <= totalRows; row++)
                        {
                            var rowData = new List<string>();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                rowData.Add(worksheet.Cells[row, col].Value?.ToString() ?? string.Empty);
                            }
                            ExcelData.Add(rowData);
                        }

                        Message = "Файл успешно загружен и обработан!";
                    }
                }
            }
            catch (Exception ex)
            {
                Message = $"Ошибка: {ex.Message}";
            }

            AddDataFromFile(ExcelData);

            return RedirectToPage("/Tournaments/Details");
        }

        public void AddDataFromFile(List<List<string>> dataFromExcel)
        {
            _context.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");

            var dateTournament = dataFromExcel[0][1];

            string[] parts = dateTournament.Split(' ');

            if (parts.Length > 1)
            {
                string dateStr = parts[1];

                if (!DateOnly.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateOnly parsedDate))
                    return;

                tournamentDate = parsedDate;
            }

            foreach (var item in dataFromExcel)
            {
                if (item[0] == string.Empty)
                    continue;

                string nickName = item[0];
                int playerId;

                if (!IsPlayerExist(nickName))
                {
                    var player = new Player { PName = nickName };
                    _context.Players.Add(player);
                    _context.SaveChanges();
                    playerId = player.Id;
                }
                else
                    playerId = _context.Players.FirstOrDefault(x => x.PName == nickName).Id;

                int countOfBasicFlags, countOfOtherFlags, countOfNotGivenFlags;

                if (item.Count <= 1 || !int.TryParse(item[1], out countOfBasicFlags))
                    countOfBasicFlags = 0;
                if (item.Count <= 2 || !int.TryParse(item[2], out countOfOtherFlags))
                    countOfOtherFlags = 0;
                if (item.Count <= 3 || !int.TryParse(item[3], out countOfNotGivenFlags))
                    countOfNotGivenFlags = 0;

                _context.FlagTournamentDetails.Add(
                    new FlagTournamentDetails { PlayerId = playerId, 
                        CountOfBasicFlags = countOfBasicFlags, 
                        CountOfOtherFlags = countOfOtherFlags,
                        CountOfNotGivenFlags = countOfNotGivenFlags, 
                        TournamentDate = tournamentDate == null ? DateOnly.MinValue : tournamentDate.Value });
                _context.SaveChanges();
            }
        }

        bool IsPlayerExist(string nickName)
        {
            return _context.Players.FirstOrDefault(x => x.PName == nickName) != null;
        }
    }
}
