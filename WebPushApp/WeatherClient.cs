namespace WebPushApp;

public class WeatherClient : BackgroundService
{
    private readonly HttpClient _httpClient = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            try
            {
                var response = await _httpClient.GetStringAsync("http://localhost:5200/WeatherForecast", stoppingToken);
                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}