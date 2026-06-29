using System.ComponentModel.DataAnnotations;

namespace EventService.Api.Data.Entities;

/// <summary>
/// Represents a single seat for an event.
/// </summary>
public class Seat
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string EventId { get; set; } = string.Empty;

    public string SeatNumber { get; set; } = string.Empty;

    public string Row { get; set; } = string.Empty;

    public int Column { get; set; }

    public string Section { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public SeatStatus Status { get; set; } = SeatStatus.Available;

    public string? ReservedByUserId { get; set; }

    public DateTime? ReservedUntil { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public EventItem? Event { get; set; }
}

/// <summary>
/// The reservation state for a seat.
/// </summary>
public enum SeatStatus
{
    Available,
    Held,
    Reserved,
    Sold
}
