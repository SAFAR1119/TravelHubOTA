using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class HomeController : Controller
{
private readonly HotelService _hotelService;

public HomeController(HotelService hotelService)
{
    _hotelService = hotelService;
}

public IActionResult Index(string? search)
{
    var hotels = _hotelService.SearchHotels(search);

    ViewBag.Search = search;

    return View(hotels);
}


}
