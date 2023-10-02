namespace WebApplication1;

public class WeatherClient : BackgroundService
{
    private readonly HttpClient _httpClient = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            var response = await _httpClient.GetStringAsync("http://localhost:5130/WeatherForecast", stoppingToken);
            Console.WriteLine(response);
        }
    }
}