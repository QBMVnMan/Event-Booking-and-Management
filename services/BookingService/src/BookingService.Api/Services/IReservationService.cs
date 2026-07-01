namespace BookingService.Api.Services;

/// <summary>
/// Handles seat hold and reservation workflows for bookings.
/// </summary>
public interface IReservationService
{
    Task<(bool Success, string Message, object? Data)> HoldSeatsAsync(string userId, string eventId, IEnumerable<string> seatIds, TimeSpan? ttl = null);
}
