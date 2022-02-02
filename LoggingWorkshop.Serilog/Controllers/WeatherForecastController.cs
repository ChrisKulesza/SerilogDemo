using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    // ILogger is the default logging abstraction built into ASP.NET Core
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Log messages with structured content embedded into msg text.
        // The following line uses the ASP.NET core logging abstraction.
        _logger.LogInformation("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

        // The following line demonstrates how we could use serilog's
        // own abstraction. Offers more features than ASP.NET core logging.

        // TODO Log with context
        //_seriLogger
        //    .ForContext("Controller", nameof(WeatherForecastController))
        //    .ForContext("Methode", nameof(Get))
        //    .Warning("Entered");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Weather.Summaries[Random.Shared.Next(Weather.Summaries.Length)]
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
            // TODO: Log an error
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
        // TODO: Log input data

        return StatusCode(StatusCodes.Status201Created);
    }
}