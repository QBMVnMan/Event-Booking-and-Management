using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? category = null, [FromQuery] string? q = null)
        {
            var result = _context.Events.AsQueryable();
            if (!string.IsNullOrWhiteSpace(category))
                result = result.Where(e =>
                    (e.Category != null && e.Category.Contains(category, StringComparison.OrdinalIgnoreCase)) ||
                    (e.Name != null && e.Name.Contains(category, StringComparison.OrdinalIgnoreCase))
                );
            if (!string.IsNullOrWhiteSpace(q))
                result = result.Where(e => e.Name != null && e.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
            return Ok(await result.ToListAsync());
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            // For demo, just return the first 2 events as featured
            return Ok(await _context.Events.Take(2).ToListAsync());
        }
    }
}
