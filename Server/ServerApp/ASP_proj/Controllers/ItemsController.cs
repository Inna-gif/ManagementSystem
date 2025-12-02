using ASP_proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteEntity = ASP_proj.Models.Route;

namespace ASP_proj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteEntity>>> GetItems()
        {
            // У нашій предметній області Items = Routes
            var items = await _context.Routes.ToListAsync();
            return Ok(items);
        }
    }
}