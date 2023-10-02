using OpenTelemetry.Resources;
using System.Reflection.PortableExecutable;
using OpenTelemetry.Metrics;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MyInstruments>();

builder.Services.AddOpenTelemetry() //Add OpenTelemetry.Extensions.Hosting nuget package
    .ConfigureResource(r => r.AddService(MyInstruments.MeterName, serviceInstanceId: Environment.MachineName))
    .WithMetrics(meterBuilder => meterBuilder
            .AddMeter(MyInstruments.MeterName)
            .AddOtlpExporter((exporterOptions, metricReaderOptions) => // Add OpenTelemetry.Exporter.OpenTelemetryProtocol nuget package
            {
                //exporterOptions.Endpoint = new Uri("http://localhost:4317/");
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
            }));
builder.Services.AddHostedService<WeatherClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
