using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using StatML.Models;
using System.Net.Mail;
using System.Net.WebSockets;
using System.IO;

namespace StatML.Pages.EmailNotifications
{
    public class SaltsModel : PageModel
    {
        private readonly AppDbContext _context;
        public void OnGet()
        {
        }

        private readonly IEmailService _emailService;

        public SaltsModel(IEmailService emailService, AppDbContext context)
        {
            _context = context;
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        [BindProperty]
        public string RecipientEmail { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Body { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2));

                var salts = _context.Salts.Include(x => x.PlayerEntity).Where(x => x.DateSalt.HasValue
                    && x.DateSalt.Value >= today && x.DateSalt.Value <= endDate).OrderBy(x => x.DateSalt).ToList();


                var body = CreateBody(salts);

                //var salts = _context.Salts.Where(x => x.DateSalt.Value.ToDateTime(TimeOnly.MaxValue) > DateTime.Today && x.DateSalt.Value.ToDateTime(TimeOnly.MaxValue) > DateTime.Today.AddDays(3));

                await _emailService.SendEmailAsync(RecipientEmail, Subject, body, true); // true дл€ HTML
                Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed at {DateTime.Now}: {ex.ToString()}");
                Message = $"Error sending email: {ex.Message}";
            }

            return Page();
        }

        public string CreateBody(IList<Salt> salts)
        {
            var rowTemplate = "<tr><td>{0}</td><td>{1}-{2}</td><td>{3}</td><td>{4}</td></tr>";

            string? rows = "";

            

            foreach (var item in salts)
            {
                var playerFrom = _context.Salts.Where(x => x.HorizontalPosition == item.HorizontalPosition
                    && x.VerticalPosition == item.VerticalPosition
                    && item.DateSalt.HasValue
                    && (item.DateSalt.Value == x.DateSalt.Value.AddDays((int)x.DaysForPlayer) 
                    || item.DateSalt.Value == x.DateSalt.Value.AddDays((int)x.DaysForPlayer + 1))).Include(x => x.PlayerEntity).FirstOrDefault();

                rows += string.Join(Environment.NewLine, string.Format(rowTemplate, playerFrom != null && playerFrom.PlayerEntity != null ? playerFrom.PlayerEntity.PName : "",
                    item.HorizontalPosition, item.VerticalPosition, item.PlayerEntity.PName, item.DateSalt?.ToString("dd.MM.yyyy")));
            }

            var emailBody = System.IO.File.ReadAllText("App_data/email_template.html").Replace("<!-- ƒинамические строки будут вставл€тьс€ здесь -->", rows)
                .Replace("[0] [1] [2] [3]", "");

            return emailBody;
        }
    }
}
