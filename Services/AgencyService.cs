using TravelHubOTA.Models;

namespace TravelHubOTA.Services;

public class AgencyService
{
    private readonly JsonService _jsonService;

    public AgencyService(JsonService jsonService)
    {
        _jsonService = jsonService;
    }

    public List<Agency> GetAllAgencies()
    {
        return _jsonService.Read<Agency>("agencies.json");
    }

    public Agency? Login(string email, string password)
    {
        var agencies = GetAllAgencies();

        return agencies.FirstOrDefault(a =>
            a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            a.Password == password);
    }

    public Agency? GetAgencyById(int id)
    {
        return GetAllAgencies()
            .FirstOrDefault(a => a.Id == id);
    }
}