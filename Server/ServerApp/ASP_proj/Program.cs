using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ASP_proj.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger (для REST API документації)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core – підключення до SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Адмінські креденшли (для MVC-логіну)
builder.Services.AddSingleton(new AdminCredentials());

// MVC (контролери + в'юшки)
builder.Services.AddControllersWithViews();

// ===== JWT-аутентифікація =====
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };

        // Тепер підтримуємо ДВА варіанти:
        // 1) Authorization: Bearer <token>  (для Postman / MAUI)
        // 2) cookie AuthToken                 (для MVC-адмінки)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Якщо є заголовок Authorization – використовуємо його
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    // JwtBearer сам витягне токен із "Bearer xxx"
                    return Task.CompletedTask;
                }

                // Інакше пробуємо взяти токен із cookie AuthToken
                var cookieToken = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(cookieToken))
                {
                    context.Token = cookieToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Обробка помилок + Swagger
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Атрибутивні маршрути для API-контролерів (ItemsController, ActionsController, ApiAuthController)
app.MapControllers();

// Маршрут для MVC (адмін-панель, логін і CRUD)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();