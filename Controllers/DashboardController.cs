using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class DashboardController : Controller
{
    private readonly AgencyService _agencyService;
    private readonly BookingService _bookingService;

    public DashboardController(
        AgencyService agencyService,
        BookingService bookingService)
    {
        _agencyService = agencyService;
        _bookingService = bookingService;
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

        if (agency == null)
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account");
        }

        var bookings = _bookingService
            .GetAllBookings()
            .Where(b => b.AgencyId == agencyId.Value)
            .ToList();

        ViewBag.Agency = agency;
        ViewBag.Bookings = bookings;

        return View();
    }
}