namespace Contracts;

/// <summary>
/// Represents the seat map returned for an event.
/// </summary>
public class SeatMapResponse
{
    public string EventId { get; set; } = string.Empty;
    public List<SeatDto> Seats { get; set; } = new();
    public int TotalSeats { get; set; }
    public int AvailableCount { get; set; }
}
