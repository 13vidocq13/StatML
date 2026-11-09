using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatML.Models;

namespace StatML.Pages.Sets
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Sett Sett { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sett = _context.Sets.Find(id);

            if (Sett == null)
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

            _context.Sets.Update(Sett);
            _context.SaveChanges();

            return RedirectToPage("/Sets/Management");
        }
    }
}
