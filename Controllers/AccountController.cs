using Microsoft.AspNetCore.Mvc;
using TravelHubOTA.Services;

namespace TravelHubOTA.Controllers;

public class AccountController : Controller
{
    private readonly AgencyService _agencyService;

    public AccountController(AgencyService agencyService)
    {
        _agencyService = agencyService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var agency = _agencyService.Login(email, password);

        if (agency == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        HttpContext.Session.SetInt32("AgencyId", agency.Id);
        HttpContext.Session.SetString("AgencyName", agency.Name);

        return RedirectToAction(
            "Index",
            "Dashboard");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(
            "Login",
            "Account");
    }
}