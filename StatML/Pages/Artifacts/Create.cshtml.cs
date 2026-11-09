using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.Artifacts
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public SelectList Sets { get; set; }

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Artifact Artifact { get; set; }
        public IActionResult OnGet()
        {
            Sets = new SelectList(_context.Sets.OrderByDescending(x => x.Level).ThenBy(x => x.Id).ToList()
                .Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),
                     Text = string.Format("{0} ({1} ур.)", x.Name, x.Level)
                 }), "Value", "Text");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Artifacts.Add(Artifact);
            _context.SaveChanges();

            return RedirectToPage("/Artifacts/Management");
        }
    }
}
