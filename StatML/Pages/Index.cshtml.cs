using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Player> Players { get; set; }

        public void OnGet()
        {
            //Test123();

            Players = _context.Players.ToList();
        }

        public void OnPost()
        {
        }

        public void Test123()
        {
            var data = new Player { Id = 2, PName = "test123", HorizontalPosition = 1, VerticalPosition = 1 };
            _context.Players.Add(data);
            _context.SaveChanges();
        }
    }
}
