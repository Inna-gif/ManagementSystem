using ASP_proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteEntity = ASP_proj.Models.Route;
using Microsoft.AspNetCore.Authorization;

namespace ASP_proj.Controllers
{
    [Authorize]
    public class RoutesController : Controller
    {
        private readonly AppDbContext _context;

        public RoutesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var routes = await _context.Routes.ToListAsync();
            return View(routes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteId == id);

            if (route == null) return NotFound();

            return View(route);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteEntity route)
        {
            if (!ModelState.IsValid)
                return View(route);

            _context.Add(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound();

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RouteEntity route)
        {
            if (id != route.RouteId) return NotFound();

            if (!ModelState.IsValid)
                return View(route);

            _context.Update(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteId == id);

            if (route == null) return NotFound();

            return View(route);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}