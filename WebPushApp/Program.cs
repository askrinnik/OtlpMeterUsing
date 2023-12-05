using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using WebPushApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<WeatherClient>();

builder.Services.AddOpenTelemetry() //Add OpenTelemetry.Extensions.Hosting nuget package
    .ConfigureResource(r => r
        .AddService(MyInstruments.MeterName, serviceInstanceId: Environment.MachineName)
        .AddAttributes(new Dictionary<string, object>
        {
            ["EnvironmentName"] = MyInstruments.GlobalSystemName, // That is visible in the target_info separate metric.
        })
        .AddTelemetrySdk()) // add resource attributes about OpenTelemetry.Sdk (optional)
    .WithMetrics(meterBuilder => meterBuilder
        .AddMyInstruments()
        .AddAspNetCoreInstrumentation() // Add OpenTelemetry.Instrumentation.AspNetCore nuget package
        .AddHttpClientInstrumentation() // Add OpenTelemetry.Instrumentation.Http nuget package
        .AddRuntimeInstrumentation() // Add OpenTelemetry.Instrumentation.Runtime nuget package
        .AddProcessInstrumentation() // Add OpenTelemetry.Instrumentation.Process nuget package
        .AddOtlpExporter((exporterOptions, metricReaderOptions) => // Add OpenTelemetry.Exporter.OpenTelemetryProtocol nuget package
            {
                //exporterOptions.Endpoint = new Uri("http://localhost:4317/");
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
            })
    );


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
