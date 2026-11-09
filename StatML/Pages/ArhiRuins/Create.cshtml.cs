using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.ArhiRuins
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public SelectList Players { get; set; }

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArhiRuin ArhiRuin { get; set; }

        public IActionResult OnGet()
        {
            var players = _context.Players.OrderBy(x => x.PName.ToLower()).ToList();
            Players = new SelectList(players, "Id", "PName");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ArhiRuins.Add(ArhiRuin);
            _context.SaveChanges();

            return RedirectToPage("/ArhiRuins/Management");
        }
    }
}
