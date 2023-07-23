using AirportReportApi.Core.Configurations;
using AirportReportApi.Core.Repositories;
using AirportReportApi.Core.Services;
using Microsoft.Extensions.Options;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Set up logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Configurations to be injected.
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<AirportWeatherConfig>(configuration.GetSection("WeatherApiConfig"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<AirportWeatherConfig>>().Value);
builder.Services.Configure<AirportDetailsConfig>(configuration.GetSection("DetailsApiConfig"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<AirportDetailsConfig>>().Value);

// Services to be injected.
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IAirportReportService, AirportReportService>();
builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddAutoMapper(typeof(Program));

// Enable CORS.
var corsOrigins = configuration.GetSection("AllowedOrigins").Get<List<string>>();
if (corsOrigins is null)
{
    throw new Exception("AllowedOrigins is not configured.");
}
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            foreach (var corsOrigin in corsOrigins)
            {
                policy.SetIsOriginAllowed(origin => new Uri(origin).Host == corsOrigin);
            }
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
