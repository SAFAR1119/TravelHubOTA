using TravelHubOTA.Models;

namespace TravelHubOTA.Services;

public class BookingService
{
    private readonly JsonService _jsonService;

    public BookingService(JsonService jsonService)
    {
        _jsonService = jsonService;
    }

    public List<Booking> GetAllBookings()
    {
        return _jsonService.Read<Booking>("bookings.json");
    }

    public Booking? GetBookingById(int id)
    {
        return GetAllBookings()
            .FirstOrDefault(b => b.Id == id);
    }

    public Booking CreateBooking(Booking booking)
    {
        var bookings = GetAllBookings();

        if (bookings.Count > 0)
        {
            booking.Id = bookings.Max(b => b.Id) + 1;
        }
        else
        {
            booking.Id = 1;
        }

        booking.Status = "Confirmed";

        bookings.Add(booking);

        _jsonService.Write("bookings.json", bookings);

        return booking;
    }
}