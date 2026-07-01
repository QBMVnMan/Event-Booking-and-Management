using System.ComponentModel.DataAnnotations;

namespace BookingService.Api.Data.Entities;

/// <summary>
/// Represents a seat reservation or hold linked to a booking.
/// </summary>
public class BookingSeat
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string BookingId { get; set; } = string.Empty;

    public string SeatId { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Status { get; set; } = "Held";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    public BookingItem? Booking { get; set; }
}
