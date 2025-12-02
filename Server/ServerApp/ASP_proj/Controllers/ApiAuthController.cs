using ASP_proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP_proj.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ApiAuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ApiAuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // DTO для логіну через API
        public class ApiLoginRequest
        {
            [Required]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Password { get; set; } = string.Empty;
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] ApiLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) шукаємо "користувача" серед водіїв
            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Email == request.Email);

            if (driver == null)
                return Unauthorized("Invalid email or password.");

            // 2) перевірка пароля
            // Поки що просте порівняння, бо в PasswordHash лежать "pass1"/"pass2".
            // Коли захочеш BCrypt – тут замінимо на BCrypt.Verify(...)
            if (driver.PasswordHash != request.Password)
                return Unauthorized("Invalid email or password.");

            // 3) формуємо JWT
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, driver.DriverId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, driver.Email),
                new Claim(ClaimTypes.Name, driver.Name),
                new Claim(ClaimTypes.Role, "User") // роль для [Authorize], якщо знадобиться
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 4) повертаємо токен MAUI/Postman'у
            return Ok(new { token = tokenString });
        }
    }
}