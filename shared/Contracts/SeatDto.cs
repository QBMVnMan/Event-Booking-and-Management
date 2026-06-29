namespace Contracts;

/// <summary>
/// Represents a seat in the read-only seat map response.
/// </summary>
public class SeatDto
{
    public string Id { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string Row { get; set; } = string.Empty;
    public int Column { get; set; }
    public string Section { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
}
