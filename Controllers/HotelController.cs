using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class HotelController : Controller
{
private readonly HotelService _hotelService;

public HotelController(HotelService hotelService)
{
    _hotelService = hotelService;
}

public IActionResult Details(int id)
{
    var hotel = _hotelService.GetHotelById(id);

    if (hotel == null)
    {
        return NotFound();
    }

    return View(hotel);
}


}
