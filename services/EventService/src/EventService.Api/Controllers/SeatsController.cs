using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Api.Controllers;

/// <summary>
/// Provides read-only seat map endpoints for events.
/// </summary>
[ApiController]
[Route("api/events")]
public class SeatsController : ControllerBase
{
    private readonly EventDbContext _context;

    public SeatsController(EventDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves the seat map for a specific event.
    /// </summary>
    [HttpGet("{eventId}/seats")]
    public async Task<IActionResult> GetSeats(string eventId)
    {
        try
        {
            var seats = await _context.Seats
                .Where(s => s.EventId == eventId)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Column)
                .Select(s => new SeatDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Row = s.Row,
                    Column = s.Column,
                    Section = s.Section,
                    Price = s.Price,
                    Status = s.Status.ToString()
                })
                .ToListAsync();

            if (seats.Count == 0)
            {
                return NotFound(new { message = "No seats found for the specified event." });
            }

            var response = new SeatMapResponse
            {
                EventId = eventId,
                Seats = seats,
                TotalSeats = seats.Count,
                AvailableCount = seats.Count(s => s.Status.Equals("Available", StringComparison.OrdinalIgnoreCase))
            };

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the seat map." });
        }
    }
}
