using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using WebPullApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<WeatherClient>();

builder.Services.AddOpenTelemetry() //Add OpenTelemetry.Extensions.Hosting nuget package
    .ConfigureResource(r => r.AddService(MyInstruments.MeterName, serviceInstanceId: Environment.MachineName))
    .WithMetrics(b => b
        .AddMyInstruments()
        .AddAspNetCoreInstrumentation() // Add OpenTelemetry.Instrumentation.AspNetCore nuget package
        .AddHttpClientInstrumentation() // Add OpenTelemetry.Instrumentation.Http nuget package
        .AddRuntimeInstrumentation() // Add OpenTelemetry.Instrumentation.Runtime nuget package
        .AddProcessInstrumentation() // Add OpenTelemetry.Instrumentation.Process nuget package
        .AddPrometheusExporter());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapControllers();

app.Run();
