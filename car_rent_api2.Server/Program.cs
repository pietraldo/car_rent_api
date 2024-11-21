using System.Security.Claims;
using car_rent_api2.Server;
using car_rent_api2.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

var sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

// Add services to the container.

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "Server=localhost;Database=car_rent;Trusted_Connection=True;TrustServerCertificate=True;");
Environment.SetEnvironmentVariable("AUTHENTICATION_GOOGLE_ID", "111973067990-qv3orig9e2d2shmib698d02ua0bgq4gl.apps.googleusercontent.com");
Environment.SetEnvironmentVariable("AUTHENTICATION_GOOGLE_SECRET", "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");

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDbContext<CarRentDbContext>(options =>
    options.UseSqlServer(connectionString));

var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_ID") ?? throw new InvalidOperationException("Missing Google API client ID");
        googleOptions.ClientSecret = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_SECRET") ?? throw new InvalidOperationException("Missing Google API secret");
        googleOptions.CallbackPath = "/api/Identity/google-login-callback";
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CarRentDbContext>()
    .AddDefaultTokenProviders().AddApiEndpoints();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.WithOrigins("https://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddHttpLogging(o => {});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager)=> {
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/pingauth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email);
    return Results.Json(new { Email = email });
}).RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseHttpLogging();

app.Run();