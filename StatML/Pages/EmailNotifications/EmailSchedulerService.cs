using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.EmailNotifications
{
    public class EmailSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailSchedulerService> _logger;
        private readonly TimeSpan _scheduleInterval = TimeSpan.FromDays(1); // Раз в сутки
        private DateTime _nextRun = DateTime.MinValue;

        private readonly AppDbContext _context;
        public IList<Salt> Salts { get; set; }
        
        public EmailSchedulerService(IServiceScopeFactory scopeFactory, ILogger<EmailSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _nextRun = DateTime.Today.AddHours(0); // Установи время запуска (например, 8:00 утра)
            if (_nextRun < DateTime.Now) _nextRun = _nextRun.AddDays(1); // Если время прошло, сдвинь на следующий день
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now >= _nextRun)
                {
                    _logger.LogInformation("Running scheduled email task at {Time}", DateTime.Now);
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        // Здесь задаёшь параметры для отправки (например, тестовый email)
                        string to = "mlalarmarhon@gmail.com"; // Замени на нужный адрес
                        string subject = "Daily Report";
                        string body = "This is your daily automated email.";
                        await emailService.SendEmailAsync(to, subject, body, true); // true для HTML
                        _logger.LogInformation("Email sent successfully at {Time}", DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send scheduled email at {Time}", DateTime.Now);
                    }
                    _nextRun = _nextRun.AddDays(1); // Устанавливаем следующий запуск через сутки
                }
                await Task.Delay(60000, stoppingToken); // Проверяем каждую минуту
           }
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

            var emailBody = System.IO.File.ReadAllText("App_data/email_template.html").Replace("<!-- Динамические строки будут вставляться здесь -->", rows)
                .Replace("[0] [1] [2] [3]", "");

            return emailBody;
        }
    }
}
