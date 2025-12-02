using ASP_proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ASP_proj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ActionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActionsController(AppDbContext context)
        {
            _context = context;
        }

        // DTO для створення "Action" (у нас це Schedule)
        public class CreateActionRequest
        {
            [Required]
            public int DriverId { get; set; }

            [Required]
            public int RouteId { get; set; }

            [Required]
            public string Status { get; set; } = string.Empty;

            // рядок, щоб у Swagger/Postman було просто "09:30"
            [Required]
            public string DepartureTime { get; set; } = string.Empty;
        }

        // POST: /api/actions
        [HttpPost]
        public async Task<ActionResult<Schedule>> CreateAction([FromBody] CreateActionRequest request)
        {
            // 1) Перевірка атрибутів [Required]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2) Перевіряємо, що Driver існує
            var driverExists = await _context.Drivers
                .AnyAsync(d => d.DriverId == request.DriverId);

            if (!driverExists)
            {
                return BadRequest("Driver with given DriverId does not exist.");
            }

            // 3) Перевіряємо, що Route існує
            var routeExists = await _context.Routes
                .AnyAsync(r => r.RouteId == request.RouteId);

            if (!routeExists)
            {
                return BadRequest("Route with given RouteId does not exist.");
            }

            // 4) Парсимо DepartureTime (рядок) у TimeSpan
            if (!TimeSpan.TryParse(request.DepartureTime, out var departure))
            {
                return BadRequest("Invalid time format. Use HH:mm, наприклад: \"09:30\".");
            }

            // 5) Створюємо Schedule як Action
            var schedule = new Schedule
            {
                DriverId = request.DriverId,
                RouteId = request.RouteId,
                DepartureTime = departure,   // <-- вже TimeSpan
                Status = request.Status
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            // 6) Повертаємо 201 Created + об'єкт
            return CreatedAtAction(
                nameof(GetActionById),
                new { id = schedule.ScheduleId },
                schedule);
        }

        // GET: /api/actions/{id} – допоміжний метод для CreatedAtAction
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetActionById(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();

            return Ok(schedule);
        }
    }
}