using System.Text.Json;

namespace TravelHubOTA.Services;

public class JsonService
{
    private readonly string _dataPath;

    public JsonService(IWebHostEnvironment environment)
    {
        _dataPath = Path.Combine(
            environment.ContentRootPath,
            "data");
    }

    public List<T> Read<T>(string fileName)
    {
        var path = Path.Combine(
            _dataPath,
            fileName);

        if (!File.Exists(path))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<T>>(json)
               ?? new List<T>();
    }

    public void Write<T>(
        string fileName,
        List<T> data)
    {
        var path = Path.Combine(
            _dataPath,
            fileName);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(
            data,
            options);

        File.WriteAllText(path, json);
    }
}