using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Players
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

        public IActionResult OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Players.Add(Player);
            _context.SaveChanges();

            return RedirectToPage("/Players/Management");
        }
    }
}
