using TravelHubOTA.Models;

namespace TravelHubOTA.Services;

public class BookingService
{
    private readonly JsonService _jsonService;
    private readonly HotelService _hotelService;

    public BookingService(
        JsonService jsonService,
        HotelService hotelService)
    {
        _jsonService = jsonService;
        _hotelService = hotelService;
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


    public List<Booking> GetBookingsByAgency(int agencyId)
{
    return GetAllBookings()
        .Where(b => b.AgencyId == agencyId)
        .OrderByDescending(b => b.Id)
        .ToList();
}

    public Booking? CreateBooking(Booking booking)
    {
        var hotel = _hotelService.GetHotelById(booking.HotelId);

        if (hotel == null)
        {
            return null;
        }

        if (booking.Rooms > hotel.RoomsAvailable)
        {
            return null;
        }

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

        _hotelService.UpdateRoomAvailability(
            booking.HotelId,
            booking.Rooms);

        return booking;
    }
}