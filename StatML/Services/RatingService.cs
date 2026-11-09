using Microsoft.EntityFrameworkCore;
using StatML.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace StatML.Services
{
    public class MlRatingResponse
    {
        [JsonPropertyName("game.service.common.FRating")]
        public FRatingData? Rating { get; set; }
    }

    public class FRatingData
    {
        [JsonPropertyName("users")]
        public UsersListContainer? Users { get; set; }
    }

    public class UsersListContainer
    {
        [JsonPropertyName("A")]
        public List<RatingItemWrapper>? Items { get; set; }
    }

    public class RatingItemWrapper
    {
        [JsonPropertyName("game.service.common.FRatingData")]
        public FRatingDataDetails? Details { get; set; }
    }

    public class FRatingDataDetails
    {
        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("ratingPoints")]
        public long RatingPoints { get; set; }

        [JsonPropertyName("user")]
        public UserWrapper? User { get; set; }
    }

    public class UserWrapper
    {
        [JsonPropertyName("game.service.user.FUserInfo")]
        public FUserInfo? Info { get; set; }
    }

    public class FUserInfo
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
    }

    public class RatingService : IRatingService
    {
        private const string ServerUrl = "https://s12.ru.mlgame.org/bridge/v2";

        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RatingService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task SyncAndSaveArmyRatingsAsync(int maxPages = 9)
        {
            var ratings = await GetTopArmyRatingsAsync(maxPages);

            if (ratings != null && ratings.Count > 0)
            {
                await SaveRatingsToDatabaseAsync(ratings);
            }
        }

        private async Task<string> GetGameSessionIdAsync()
        {
            var username = _configuration["GameSettings:Username"];
            var password = _configuration["GameSettings:Password"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Логин или пароль игры не указаны в appsettings.json.");
            }

            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true // Можешь поставить false для проверки
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            string capturedSessionId = null;

            // Перехватываем сетевые запросы: ищем тот, который идет на /bridge/v2 с gamesessionid
            page.Request += (_, request) =>
            {
                if (capturedSessionId == null && request.Url.Contains("/bridge/v2") && request.Url.Contains("gamesessionid="))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(request.Url, @"gamesessionid=([^&/]+)");
                    if (match.Success)
                    {
                        capturedSessionId = match.Groups[1].Value;
                    }
                }
            };

            // 1. Идем на портал и логинимся
            await page.GotoAsync("https://ru.mlgame.org/");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await page.FillAsync("input[name='username']", username);
            await page.FillAsync("input[name='password']", password);
            await page.ClickAsync("#loginButton");

            // 2. Ждем появления списка миров
            await page.WaitForSelectorAsync("div.light-border-box", new PageWaitForSelectorOptions { Timeout = 15000 });
            await Task.Delay(2000);

            // 3. Кликаем на кнопку входа в «Молкар» (s12)
            var serverEnterButton = page.Locator("div.light-border-box:has-text('Молкар') div#enterButton");
            await serverEnterButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
            await serverEnterButton.EvaluateAsync("node => node.click()");

            // 4. Ждем загрузки интерфейса мира
            await page.WaitForSelectorAsync("#ratingButton", new PageWaitForSelectorOptions { Timeout = 20000 });
            await Task.Delay(1000);

            // 5. Кликаем по кнопке рейтинга — в этот момент игра отправит запрос на /bridge/v2 со всеми параметрами
            var ratingButton = page.Locator("#ratingButton");
            await ratingButton.EvaluateAsync("node => node.click()");

            // 6. Ждем, пока перехватчик поймает gamesessionid из летящего запроса
            for (int i = 0; i < 30; i++)
            {
                if (!string.IsNullOrEmpty(capturedSessionId))
                {
                    break;
                }
                await Task.Delay(500);
            }

            if (string.IsNullOrEmpty(capturedSessionId))
            {
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = "login_error.png" });
                throw new Exception("Не удалось перехватить gamesessionid из запроса к бэкенду. Проверьте login_error.png");
            }

            return capturedSessionId;
        }

        public async Task<List<PlayerRatingDto>> GetTopArmyRatingsAsync(int maxPages = 9)
        {
            var gameSessionId = await GetGameSessionIdAsync();

            var resultList = new List<PlayerRatingDto>();
            var requestUrl = $"{ServerUrl};gamesessionid={gameSessionId}";

            for (int page = 0; page < maxPages; page++)
            {
                var formData = new Dictionary<string, string>
                {
                    { "service", "userService.getRating" },
                    { "params[0]", "{\"game.service.common.RatingType\":\"ARMY_POWER\"}" },
                    { "params[1]", "" },
                    { "params[2]", page.ToString() },
                    { "params[3]", "false" }
                };

                var content = new FormUrlEncodedContent(formData);

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync(requestUrl, content);
                if (!response.IsSuccessStatusCode) break;

                var jsonString = await response.Content.ReadAsStringAsync();
                var parsed = JsonSerializer.Deserialize<MlRatingResponse>(jsonString);
                var items = parsed?.Rating?.Users?.Items;

                if (items == null || items.Count == 0) break;

                foreach (var item in items)
                {
                    var details = item.Details;
                    var userInfo = details?.User?.Info;

                    if (details != null && userInfo != null && !string.IsNullOrWhiteSpace(userInfo.Username))
                    {
                        resultList.Add(new PlayerRatingDto
                        {
                            Username = userInfo.Username.Trim(),
                            ArmyPower = details.RatingPoints,
                            SnapshotDate = DateTime.UtcNow
                        });
                    }
                }

                await Task.Delay(200);
            }

            return resultList;
        }

        public async Task SaveRatingsToDatabaseAsync(List<PlayerRatingDto> ratings)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var usernames = ratings.Select(r => r.Username).Distinct().ToList();

            var existingPlayers = await _context.Players
                .Where(p => usernames.Contains(p.PName))
                .ToDictionaryAsync(p => p.PName);

            foreach (var dto in ratings)
            {
                if (!existingPlayers.TryGetValue(dto.Username, out var player))
                {
                    player = new Player
                    {
                        PName = dto.Username
                    };
                    _context.Players.Add(player);
                    existingPlayers[dto.Username] = player;
                }

                var ratingEntry = new RatingArmy
                {
                    PlayerEntity = player,
                    Rate = dto.ArmyPower,
                    RateDate = today
                };

                _context.RatingArmies.Add(ratingEntry);
            }

            await _archiveOrSaveNewRecordsAsync();
        }

        private async Task _archiveOrSaveNewRecordsAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}