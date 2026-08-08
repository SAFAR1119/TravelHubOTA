using System.ComponentModel.DataAnnotations;

namespace TravelHubOTA.Models;

public class Booking
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public int AgencyId { get; set; }

    [Required]
    public string GuestName { get; set; } = "";

    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Range(1, 20)]
    public int Rooms { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Confirmed";
}