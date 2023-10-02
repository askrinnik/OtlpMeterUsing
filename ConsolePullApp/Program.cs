using Prometheus;
using System.Diagnostics.Metrics;

using var server = new KestrelMetricServer(port: 9184);
server.Start();

var rand = Random.Shared;

var currentHeartRate = 70;

var meter = new Meter("MeterUsing.Human");

var heartRateGauge = meter.CreateObservableGauge("heart-rate", () => currentHeartRate, "Hits per second", "The heart rate");
var distanceCounter = meter.CreateCounter<int>("distance", "meters", "The traveled distance");

var transport = new[]{ "walk", "bus", "bicycle"};

Console.WriteLine("Press any key to exit");
while (!Console.KeyAvailable)
{
    await Task.Delay(rand.Next(200, 1000));
    currentHeartRate += rand.Next(-5, 6);
    Console.WriteLine($"Heart rate: {currentHeartRate}");
    distanceCounter.Add(rand.Next(0, 100), new KeyValuePair<string, object?>("transport", transport[rand.Next(transport.Length)]));
}