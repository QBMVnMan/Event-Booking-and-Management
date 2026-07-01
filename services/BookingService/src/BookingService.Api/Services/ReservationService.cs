using BookingService.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Api.Services;

/// <summary>
/// Simple reservation service that creates temporary holds for selected seats.
/// </summary>
public class ReservationService : IReservationService
{
    private readonly BookingDbContext _context;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(BookingDbContext context, ILogger<ReservationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool Success, string Message, object? Data)> HoldSeatsAsync(string userId, string eventId, IEnumerable<string> seatIds, TimeSpan? ttl = null)
    {
        try
        {
            var seatList = seatIds?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? new List<string>();
            if (seatList.Count == 0)
            {
                return (false, "No seats selected.", null);
            }

            var booking = new BookingItem
            {
                Id = Guid.NewGuid().ToString(),
                EventId = eventId,
                UserId = userId,
                Quantity = seatList.Count,
                Note = "Held by reservation service"
            };

            _context.Bookings.Add(booking);

            var expiresAt = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromMinutes(10));
            var bookingSeats = seatList.Select(seatId => new BookingSeat
            {
                Id = Guid.NewGuid().ToString(),
                BookingId = booking.Id,
                SeatId = seatId,
                EventId = eventId,
                UserId = userId,
                Status = "Held",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            }).ToList();

            _context.Set<BookingSeat>().AddRange(bookingSeats);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Held {SeatCount} seats for event {EventId} by user {UserId}", seatList.Count, eventId, userId);

            return (true, "Seats held successfully.", new { bookingId = booking.Id, seats = bookingSeats.Select(s => s.SeatId) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to hold seats for event {EventId} by user {UserId}", eventId, userId);
            return (false, "An error occurred while holding seats.", null);
        }
    }
}
