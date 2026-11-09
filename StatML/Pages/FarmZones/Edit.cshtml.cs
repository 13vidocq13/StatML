using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.FarmZones
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player = _context.Players.Find(id);

            if (Player == null)
            {
                return NotFound();
            }

            var farmZoneEntities = _context.FarmZones.Where(x => x.PlayerId == id).ToList();

            string farmZone = string.Join(", ", farmZoneEntities.Select(x => $"{x.HorizontalPosition}-{x.VerticalPosition}"));

            ViewData["farmZone"] = farmZone;

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var farmZones = Request.Form["FarmZone"].ToString().Split(',');

                    var farmZoneEntities = _context.FarmZones.Where(x => x.PlayerId == id).ToList();

                    foreach (var item in farmZoneEntities)
                    {
                        _context.FarmZones.Remove(item);
                        _context.SaveChanges();
                    }

                    foreach (var item in farmZones)
                    {
                        var coors = item.Split('-');

                        int horizontalPosition;
                        int verticalPosition;

                        if (!int.TryParse(coors[0], out horizontalPosition) || !int.TryParse(coors[1], out verticalPosition))
                            continue;

                        FarmZone farmZone = new FarmZone
                        {
                            PlayerId = id,
                            HorizontalPosition = horizontalPosition,
                            VerticalPosition = verticalPosition
                        };

                        _context.FarmZones.Update(farmZone);
                        _context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return RedirectToPage("/FarmZones/Management");
        }
    }
}
