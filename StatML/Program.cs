using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using StatML.Models;
using StatML.Pages.EmailNotifications;
using StatML.Services;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var supportedCultures = new[] { new CultureInfo("ru-RU") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddScoped<AuthService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Регистрация сервиса сбора рейтинга
builder.Services.AddScoped<IRatingService, RatingService>();

// add DbContext
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация кастомного сервиса email
builder.Services.AddScoped<IEmailService, CustomEmailService>();
builder.Services.AddHostedService<EmailSchedulerService>(); // Регистрация фоновой задачи

// Настройка чистой схемы кук для кастомной таблицы Logins (работает и на локалхосте, и на HTTP-хостинге)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Ключевой момент для HTTP хостинга
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Настройка авторизации по ролям
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SaltsEditor", policy => policy.RequireRole("Admin", "SaltsEditor"));
    options.AddPolicy("GrailsEditor", policy => policy.RequireRole("Admin", "GrailsEditor"));
    options.AddPolicy("TournamentsEditor", policy => policy.RequireRole("Admin", "TournamentEditor"));
    options.AddPolicy("Observer", policy => policy.RequireRole("Admin", "User", "SaltsEditor", "GrailsEditor"));
    options.AddPolicy("Everyone", policy => policy.RequireRole("Admin", "User", "SaltsEditor", "GrailsEditor", "TournamentEditor"));
});

builder.Services.AddHostedService<RatingBackgroundService>();

var app = builder.Build();

app.MapGet("/run-sync", async ([Microsoft.AspNetCore.Mvc.FromServices] IRatingService ratingService) =>
{
    try
    {
        await ratingService.SyncAndSaveArmyRatingsAsync();
        return Results.Ok("Сервис успешно отработал! Данные скачаны из игры и записаны в базу.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Ошибка при синхронизации: {ex.Message}");
    }
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Migration applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


// --- Кастомные сервисы ---

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}

public class CustomEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public CustomEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Напоминание о передаче озер", _configuration["MailSettings:SenderEmail"]));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart(isHtml ? "html" : "plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["MailSettings:Host"], int.Parse(_configuration["MailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["MailSettings:SenderEmail"], _configuration["MailSettings:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

public class AuthService
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }
}