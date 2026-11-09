using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.FarmZones
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var farmZones = Request.Form["FarmZone"].ToString().Split(',');

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

                        _context.FarmZones.Add(farmZone);
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
