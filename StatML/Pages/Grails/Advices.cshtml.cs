using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Grails
{
    public class AdvicesModel : PageModel
    {
        //20 королевств от граал€ допустимое рассто€ние
        const int range = 16;
        private readonly AppDbContext _context;
        public AdvicesModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "¬ведите координату X.")]
        public int InputX { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "¬ведите координату Y.")]
        public int InputY { get; set; }

        public List<Player>? Players { get; set; }
        public Player? FarmZoneOwner { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            //Players = null;
        }

        public IActionResult OnPost(int InputX, int InputY)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var players = _context.Players.Include(p => p.GrailEntities).Include(p => p.SaltEntities)
                .Where(x => x.HorizontalPosition >= InputX - range && x.HorizontalPosition <= InputX + range 
                    && x.VerticalPosition >= InputY - range && x.VerticalPosition <= InputY + range
                    && x.IsGeneral == true).ToList();

            var sortedPlayers = players
                .Select(p => new
                {
                    Player = p,
                    TotalHours = (p.GrailEntities?.Sum(g => g.HoursForPlayer * 200) ?? 0) + (p.SaltEntities?.Sum(s => s.DaysForPlayer * 20 * 24) ?? 0)
                })
                .OrderBy(x => x.TotalHours)
                .Select(x => x.Player) 
                .ToList();

            Players = sortedPlayers;

            var farmZoneOwnerId = _context.FarmZones.Where(x => x.HorizontalPosition == InputX && x.VerticalPosition == InputY).Select(x => x.PlayerId).FirstOrDefault();

            FarmZoneOwner = _context.Players.FirstOrDefault(x => x.Id == farmZoneOwnerId);

            if (!Players.Any())
            {
                Message = "»гроки не найдены.";
            }

            return Page();
        }
    }
}
