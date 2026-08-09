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

        if (booking.Rooms <= 0)
        {
            return null;
        }

        if (booking.Rooms > hotel.RoomsAvailable)
        {
            return null;
        }

        var bookings = GetAllBookings();

        booking.Id = bookings.Count > 0
            ? bookings.Max(b => b.Id) + 1
            : 1;

        booking.Status = "Confirmed";

        bookings.Add(booking);

        // Save booking
        _jsonService.Write("bookings.json", bookings);

        // Reduce available rooms
        _hotelService.UpdateRoomAvailability(
            booking.HotelId,
            booking.Rooms);

        return booking;
    }

    public bool CancelBooking(int bookingId, int agencyId)
    {
        var bookings = GetAllBookings();

        var booking = bookings.FirstOrDefault(
            b => b.Id == bookingId);

        if (booking == null)
        {
            return false;
        }

        // Security check
        if (booking.AgencyId != agencyId)
        {
            return false;
        }

        // Already cancelled
        if (booking.Status == "Cancelled")
        {
            return false;
        }

        // Get the hotel before changing anything
        var hotel = _hotelService.GetHotelById(
            booking.HotelId);

        if (hotel == null)
        {
            return false;
        }

        // Return rooms to inventory
        hotel.RoomsAvailable += booking.Rooms;

        // Mark booking as cancelled
        booking.Status = "Cancelled";

        // Save both updated files
        _jsonService.Write(
            "bookings.json",
            bookings);

        _hotelService.SaveHotels(
            _hotelService.GetAllHotels()
                .Select(h =>
                {
                    if (h.Id == hotel.Id)
                    {
                        h.RoomsAvailable = hotel.RoomsAvailable;
                    }

                    return h;
                })
                .ToList());

        return true;
    }
}