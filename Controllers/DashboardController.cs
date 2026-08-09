using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class DashboardController : Controller
{
    private readonly BookingService _bookingService;
    private readonly AgencyService _agencyService;

    public DashboardController(
        BookingService bookingService,
        AgencyService agencyService)
    {
        _bookingService = bookingService;
        _agencyService = agencyService;
    }

    public IActionResult Index()
    {
        var agencyId = HttpContext.Session.GetInt32("AgencyId");

        if (agencyId == null)
        {
            return RedirectToAction(
                "Login",
                "Account");
        }

        var agency = _agencyService.GetAgencyById(
            agencyId.Value);

        var bookings = _bookingService
            .GetBookingsByAgency(agencyId.Value);

        ViewBag.Agency = agency;
        ViewBag.Bookings = bookings;

        // Calculate confirmed booking value
        var totalBookingValue = bookings
            .Where(b => b.Status == "Confirmed")
            .Sum(b => b.TotalPrice);

        ViewBag.TotalBookingValue = totalBookingValue;

        return View();
    }
}