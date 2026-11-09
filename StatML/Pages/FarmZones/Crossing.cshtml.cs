using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.FarmZones
{
    public class CrossingModel : PageModel
    {
        public IList<CrossingFarmZones> CrossingFarmZones { get; set; }
        private readonly AppDbContext _context;
        public CrossingModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var duplicateZones = _context.FarmZones
                .Include(x => x.PlayerEntity)
                .Where(fz => _context.FarmZones
                    .Where(fz2 =>
                        fz2.HorizontalPosition == fz.HorizontalPosition &&
                        fz2.VerticalPosition == fz.VerticalPosition &&
                        fz2.PlayerId != fz.PlayerId
                    )
                    .Any()
                )
                .ToList();

            var CrossingFarmZonesList = new List<CrossingFarmZones>();

            var uniqueCoords = duplicateZones.GroupBy(fz => new { fz.HorizontalPosition, fz.VerticalPosition })
                .Select(x => new 
                {
                    HorizontalPosition = x.Key.HorizontalPosition,
                    VerticalPosition = x.Key.VerticalPosition
                });

            foreach (var item in uniqueCoords)
            {
                var players = duplicateZones.Where(x => x.HorizontalPosition == item.HorizontalPosition && x.VerticalPosition == item.VerticalPosition)
                    .Select(x => x.PlayerEntity).ToList();

                var crossingFarmZone = new CrossingFarmZones();
                crossingFarmZone.HorizontalPosition = item.HorizontalPosition;
                crossingFarmZone.VerticalPosition = item.VerticalPosition;

                var playersList = new List<Player>();
                
                foreach (var player in players)
                {
                    playersList.Add(player);   
                }

                crossingFarmZone.Players = playersList;

                CrossingFarmZonesList.Add(crossingFarmZone);

                //Console.WriteLine($"UserId: {item.PlayerId}, X: {item.HorizontalPosition}, Y: {item.VerticalPosition}");
            }

            CrossingFarmZones = CrossingFarmZonesList;
        }
    }

    public class CrossingFarmZones
    {
        public IList<Player> Players { get; set; }
        public int HorizontalPosition { get; set; }
        public int VerticalPosition { get; set; }
    }
}
