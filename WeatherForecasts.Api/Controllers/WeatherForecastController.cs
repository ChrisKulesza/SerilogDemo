using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    // ILogger is the default logging abstraction built into ASP.NET Core
    private readonly ILogger<WeatherForecastController> _logger;

    // Serilog.ILogger is the native logger from serilog.
    private readonly Serilog.ILogger _seriLogger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Serilog.ILogger seriLogger)
    {
        _logger = logger;
        _seriLogger = seriLogger;
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    //weatherForecast/getWeatherForecast
    public IEnumerable<WeatherForecast> Get()
    {
        // Log messages with structured content embedded into msg text.
        // The following line uses the ASP.NET core logging abstraction.
        _logger.LogInformation("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

        // The following line demonstrates how we could use serilog's
        // own abstraction. Offers more features than ASP.NET core logging.
        _seriLogger
            .ForContext("Controller", nameof(WeatherForecastController))
            .ForContext("Methode", nameof(Get))
            .Warning("Entered");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    // Endpoint that throws a handled exception
    [HttpGet("exception")]
    //weatherForecast/exception
    public IActionResult HandleException()
    {
        try
        {
            throw new InvalidOperationException("Something bad happened");
        }
        catch (InvalidOperationException ioex)
        {
            _seriLogger.Error(ioex, "Error in {Methode}", nameof(HandleException));
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // Endpoint that throws a unhandled exception
    [HttpGet("unhandled-exception")]
    public IActionResult UnhandledException() => throw new InvalidOperationException("Something bad happend.");

    // POST
    [HttpPost("customers")]
    public IActionResult CreateCustomer(CustomerDto customer)
    {
        // Simulate adding to Db, structured logging (not working with string interpolation)
        _seriLogger.Information("Writing customer {Customer} to Db", customer.Name);

        return StatusCode(StatusCodes.Status201Created);
    }
}

public record CustomerDto(
    [Required][MaxLength(60)]string Name,
    [Range(0, 100)]int Age);

//public class CustomerDtoClass
//{
//    [Required]
//    [MaxLength(50)]
//    public string Name { get; set; } = String.Empty;

//    [Range(0, 100)]
//    public int Age { get; set; }
//}