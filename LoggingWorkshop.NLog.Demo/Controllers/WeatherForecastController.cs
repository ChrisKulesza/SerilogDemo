using Microsoft.AspNetCore.Mvc;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController()
    {
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // TODO Log

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Weather.Summaries[Random.Shared.Next(Weather.Summaries.Length)]
        })
        .ToArray();
    }

    // Checking out loggin levels
    [HttpGet("levels")]
    public IActionResult GetLogLevels()
    {
        // TODO Log

        return Ok();
    }

    // Logging categories
    [HttpGet("categories")]
    public IActionResult GetLogCategories()
    {
        // TODO Log

        return Ok();
    }

    // Log scope
    [HttpGet("scopes")]
    public IActionResult GetLogScopes([FromQuery] string? sort)
    {
        // TODO Log

        return Ok();
    }

    // Endpoint that throws a handled exception
    [HttpGet("exception")]
    public IActionResult HandleException()
    {
        try
        {
            throw new InvalidOperationException("Something bad happened");
        }
        catch (InvalidOperationException ioex)
        {
            // TODO Log

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
        // TODO Log

        return StatusCode(StatusCodes.Status201Created);
    }
}