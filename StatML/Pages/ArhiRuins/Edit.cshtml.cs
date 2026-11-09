using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.ArhiRuins
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public SelectList Players { get; set; }

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArhiRuin ArhiRuin { get; set; }

        public IActionResult OnGet(int? id)
        {
            var players = _context.Players.OrderBy(x => x.PName.ToLower()).ToList();
            Players = new SelectList(players, "Id", "PName");

            ArhiRuin = _context.ArhiRuins.Find(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ArhiRuins.Update(ArhiRuin);
            _context.SaveChanges();

            return RedirectToPage("/ArhiRuins/Management");
        }
    }
}
