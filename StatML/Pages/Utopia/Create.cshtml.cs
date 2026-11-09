using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Utopia
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        //[BindProperty]
        //[Required(ErrorMessage = "Выберите первого игрока")]
        //[Range(1, int.MaxValue, ErrorMessage = "Выберите действительного игрока")]
        //public int? Player1Id { get; set; }

        //[BindProperty]
        //[Required(ErrorMessage = "Выберите второго игрока")]
        //[Range(1, int.MaxValue, ErrorMessage = "Выберите действительного игрока")]
        //public int? Player2Id { get; set; }

        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art1lvl { get; set; }
        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art2lvl { get; set; }
        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art3lvl { get; set; }
        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art4lvl { get; set; }
        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art5lvl { get; set; }
        //[BindProperty]
        //[Range(3, 5, ErrorMessage = "Артефакты в утопии могут быть только 3-5 уровней")]
        //public int Art6lvl { get; set; }

        //[BindProperty]
        //[Required(ErrorMessage = "Укажите дату утопии")]
        //public DateOnly? DateOnly { get; set; }

        public SelectList Players { get; set; }
        [BindProperty]
        public Models.Utopia Utopia { get; set; }

        public IActionResult OnGet()
        {
            var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToUpper()).ToList();

            int separatorIndex = players.FindIndex(x => x.IsGeneral == false);

            if (separatorIndex > 0 && separatorIndex < players.Count)
            {
                players.Insert(separatorIndex, new Player
                {
                    Id = -1,
                    PName = "-- Не в распределении -- ", // Текст разделителя
                   
                });
            }

            //// Создаем список с кастомными атрибутами
            //var selectListItems = players.Select(p => new SelectListItem
            //{
            //    Value = p.Id.ToString(),
            //    Text = p.PName,
            //    Disabled = p.Id == -1 // Отключаем опцию с Id = -1
            //}).ToList();

            Players = new SelectList(players, "Id", "PName");
            
            if (Players == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var players = _context.Players.OrderByDescending(x => x.IsGeneral).ThenBy(x => x.PName.ToUpper()).ToList();

                int separatorIndex = players.FindIndex(x => x.IsGeneral == false);

                if (separatorIndex > 0 && separatorIndex < players.Count)
                {
                    players.Insert(separatorIndex, new Player
                    {
                        Id = -1,
                        PName = "-- Не в распределении -- ", // Текст разделителя

                    });
                }

                Players = new SelectList(players, "Id", "PName");
                return Page();
            }

            _context.Utopias.Add(Utopia);
            _context.SaveChanges();

            return RedirectToPage("/Utopia/Management");
        }
    }
}
