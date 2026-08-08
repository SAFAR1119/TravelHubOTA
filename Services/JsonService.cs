using System.Text.Json;

namespace TravelHubOTA.Services;

public class JsonService
{
    private readonly IWebHostEnvironment _environment;

    public JsonService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(_environment.ContentRootPath, "data", fileName);
    }

    public List<T> Read<T>(string fileName)
    {
        var path = GetFilePath(fileName);

        if (!File.Exists(path))
            return new List<T>();

        var json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json)
               ?? new List<T>();
    }

    public void Write<T>(string fileName, List<T> data)
    {
        var path = GetFilePath(fileName);

        var json = JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(path, json);
    }
}