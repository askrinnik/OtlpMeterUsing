using System.Diagnostics.Metrics;

namespace WebPushApp;

public class MyInstruments : IDisposable
{
    internal const string MeterName = "MeterUsing.AspNetCore";
    private readonly Meter _meter;
    private int _todayTemperature;
    private string _todaySummary = string.Empty;
    private readonly ObservableGauge<int> _todayTemperatureGauge;

    public Counter<int> RequestsCounter { get; }

    public MyInstruments()
    {
        _meter = new Meter(MeterName);
        RequestsCounter = _meter.CreateCounter<int>("web-requests", "requests", "The number of requests to the API");
        _todayTemperatureGauge = _meter.CreateObservableGauge<int>("today-temperature", GetTemperature, "Celsius", "The temperature today");
    }

    public void SetTodayTemperature(int temperature, string summary)
    {
        _todayTemperature = temperature;
        _todaySummary = summary;
    }

    private Measurement<int> GetTemperature() => 
        new(_todayTemperature, new KeyValuePair<string, object?>("summary", _todaySummary));

    public void Dispose()
    {
        _meter.Dispose();
        GC.SuppressFinalize(this);
    }
}