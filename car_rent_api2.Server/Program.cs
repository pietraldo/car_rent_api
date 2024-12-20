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
using car_rent_api2.Server.Services;
using System.Text.Json.Serialization;
using car_rent.Server.Model;

var builder = WebApplication.CreateBuilder(args);

var sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

// Add services to the container.

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("Missing DB_CONNECTION_STRING");

builder.Services.AddDbContext<CarRentDbContext>(options =>
    options.UseSqlServer(connectionString));

// Authentication and Authorization
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_ID") ?? throw new InvalidOperationException("Missing Google API client ID");
        googleOptions.ClientSecret = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_SECRET") ?? throw new InvalidOperationException("Missing Google API secret");
        googleOptions.CallbackPath = "/api/Identity/signin-google";
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddAuthorization();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CarRentDbContext>()
    .AddDefaultTokenProviders().AddApiEndpoints();

builder.Services.AddHttpLogging(o => {});

// Add services
builder.Services.AddSingleton<IOfferManager, OfferManager>();
builder.Services.AddSingleton<IEmailService, MailGunEmailService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


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

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseHttpLogging();

app.Run();