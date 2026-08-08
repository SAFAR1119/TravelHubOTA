namespace TravelHubOTA.Models;

public class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string City { get; set; } = "";

    public string Country { get; set; } = "";

    public int Stars { get; set; }

    public decimal PricePerNight { get; set; }

    public int RoomsAvailable { get; set; }

    public string Image { get; set; } = "";

    public List<string> Facilities { get; set; } = new();
}