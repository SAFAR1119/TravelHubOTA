using TravelHubOTA.Models;

namespace TravelHubOTA.Services;

public class HotelService
{
    private readonly JsonService _jsonService;

    public HotelService(JsonService jsonService)
    {
        _jsonService = jsonService;
    }

    public List<Hotel> GetAllHotels()
    {
        return _jsonService.Read<Hotel>("hotels.json");
    }

    public Hotel? GetHotelById(int id)
    {
        return GetAllHotels()
            .FirstOrDefault(h => h.Id == id);
    }

    public List<Hotel> SearchHotels(string? search)
    {
        var hotels = GetAllHotels();

        if (string.IsNullOrWhiteSpace(search))
        {
            return hotels;
        }

        search = search.Trim().ToLower();

        return hotels
            .Where(h =>
                h.Name.ToLower().Contains(search) ||
                h.City.ToLower().Contains(search) ||
                h.Country.ToLower().Contains(search))
            .ToList();
    }
}