using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StatML.Services
{
    public class RatingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RatingBackgroundService> _logger;

        public RatingBackgroundService(IServiceProvider serviceProvider, ILogger<RatingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновый сервис синхронизации рейтингов запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan delay;

                try
                {
                    // Определяем европейскую тайм-зону
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    var nowInEurope = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

                    // Цель: ровно 00:00 следующих суток
                    var nextRunInEurope = nowInEurope.Date.AddDays(1);

                    delay = nextRunInEurope - nowInEurope;

                    _logger.LogInformation($"Следующий плановый сбор рейтинга (по Европе) запланирован на: {nextRunInEurope} (через {delay.Hours} ч. {delay.Minutes} мин.)");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалось определить европейскую тайм-зону. Используем локальное время сервера.");
                    var now = DateTime.Now;
                    delay = now.Date.AddDays(1) - now;
                }

                try
                {
                    // Ожидание до полуночи
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break; // Приложение останавливается
                }

                // Запуск плановой синхронизации
                try
                {
                    _logger.LogInformation("Запуск ночной синхронизации рейтингов...");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var ratingService = scope.ServiceProvider.GetRequiredService<IRatingService>();
                        await ratingService.SyncAndSaveArmyRatingsAsync();
                    }

                    _logger.LogInformation("Ночная синхронизация успешно завершена.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка во время фоновой синхронизации рейтингов.");
                }
            }
        }
    }
}