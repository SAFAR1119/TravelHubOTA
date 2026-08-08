using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Models;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class BookingController : Controller
{
    private readonly BookingService _bookingService;
    private readonly HotelService _hotelService;

    public BookingController(
        BookingService bookingService,
        HotelService hotelService)
    {
        _bookingService = bookingService;
        _hotelService = hotelService;
    }

    [HttpGet]
    public IActionResult Create(int hotelId)
    {
        var hotel = _hotelService.GetHotelById(hotelId);

        if (hotel == null)
        {
            return NotFound();
        }

        ViewBag.Hotel = hotel;

        return View();
    }

    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        var hotel = _hotelService.GetHotelById(booking.HotelId);

        if (hotel == null)
        {
            return NotFound();
        }

        if (booking.CheckOut <= booking.CheckIn)
        {
            ModelState.AddModelError(
                "CheckOut",
                "Check-out date must be after check-in date.");
        }

        if (booking.Rooms <= 0)
        {
            ModelState.AddModelError(
                "Rooms",
                "At least one room must be selected.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Hotel = hotel;
            return View(booking);
        }

        var nights = (booking.CheckOut - booking.CheckIn).Days;

        booking.TotalPrice =
            nights *
            booking.Rooms *
            hotel.PricePerNight;

        booking.AgencyId = 1;

        var createdBooking = _bookingService.CreateBooking(booking);

        return RedirectToAction(
            "Confirmation",
            new { id = createdBooking.Id });
    }

    public IActionResult Confirmation(int id)
    {
        var booking = _bookingService.GetBookingById(id);

        if (booking == null)
        {
            return NotFound();
        }

        var hotel = _hotelService.GetHotelById(booking.HotelId);

        if (hotel == null)
        {
            return NotFound();
        }

        ViewBag.Hotel = hotel;

        return View(booking);
    }
}