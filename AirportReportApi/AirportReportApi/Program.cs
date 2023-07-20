using AirportReportApi.Core.Data;
using AirportReportApi.Core.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection.

builder.Services.AddHttpClient("AirportWeatherClient", client =>
{
    // Get configuration from appsettings.json.
    string? weatherApiUrl = builder.Configuration["WeatherApiUrl"];
    if (weatherApiUrl is not null)
    {
        client.BaseAddress = new Uri(weatherApiUrl);
    }
    
    var headers = builder.Configuration.GetSection("WeatherApiHeaders").Get<IEnumerable<Dictionary<string, string>>>();
    if (headers is null) return;
    foreach (var header in headers)
    {
        client.DefaultRequestHeaders.Add(header["Key"], header["Value"]);
    }
});
builder.Services.AddScoped<IAirportReportService, AirportReportService>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

