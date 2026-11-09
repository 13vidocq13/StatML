using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatML.Models;

namespace StatML.Pages.Artifacts
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Artifact Artifact { get; set; }
        public SelectList Sets { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Artifact = _context.Artifacts.Find(id);

            Sets = new SelectList(_context.Sets.OrderByDescending(x => x.Level).ThenBy(x => x.Id).ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = string.Format("{0} ({1} ур.)", x.Name, x.Level)
                }), "Value", "Text");

            if (Artifact == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Sets = new SelectList(_context.Sets.OrderByDescending(x => x.Level).OrderBy(x => x.Id).ToList()
                        , "Id", "Name");

                return Page();
            }

            _context.Artifacts.Update(Artifact);
            _context.SaveChanges();

            return RedirectToPage("/Artifacts/Management");
        }
    }
}
