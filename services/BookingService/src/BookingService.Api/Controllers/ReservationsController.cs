using BookingService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers;

/// <summary>
/// Handles temporary seat holds for the seat selection flow.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    /// <summary>
    /// Holds seats temporarily for a user.
    /// </summary>
    [HttpPost("hold")]
    public async Task<IActionResult> Hold([FromBody] HoldReservationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.EventId) || request.SeatIds == null || request.SeatIds.Count == 0)
        {
            return BadRequest(new { message = "UserId, EventId, and at least one SeatId are required." });
        }

        try
        {
            var result = await _reservationService.HoldSeatsAsync(request.UserId, request.EventId, request.SeatIds);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing seat hold request.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing the hold request." });
        }
    }
}

public class HoldReservationRequest
{
    public string UserId { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public List<string> SeatIds { get; set; } = new();
}
