using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Promotions
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Promotion Promotion { get; set; }
        
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Promotion = _context.Promotions.Find(id);

            if (Promotion == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Promotions.Update(Promotion);
            _context.SaveChanges();

            return RedirectToPage("/Promotions/Management");
        }
    }
}
