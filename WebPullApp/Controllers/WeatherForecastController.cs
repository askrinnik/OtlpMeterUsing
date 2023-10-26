using Microsoft.AspNetCore.Mvc;

namespace WebPullApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly MyInstruments _myInstruments;

    public WeatherForecastController(MyInstruments myInstruments)
    {
        _myInstruments = myInstruments;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        _myInstruments.RequestsCounter.Add(1);

        switch (Random.Shared.Next(0, 10))
        {
            case 0:
                return NotFound();
            case 1:
                return StatusCode(500);
        }

        var weatherForecasts = Enumerable.Range(1, 5).Select(index =>
            {
                var temperatureC = Random.Shared.Next(-20, 40);
                return new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = temperatureC,
                    Summary = GetSummary(temperatureC)
                };
            })
            .ToArray();
        _myInstruments.SetTodayTemperature(weatherForecasts.First().TemperatureC, weatherForecasts.First().Summary!);
        return weatherForecasts;
    }

    private static string GetSummary(int temperatureC) =>
        temperatureC switch
        {
            < 10 => "Cold",
            > 30 => "Hot",
            _ => "Normal"
        };
}