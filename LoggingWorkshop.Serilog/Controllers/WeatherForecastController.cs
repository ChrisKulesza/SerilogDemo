using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly Serilog.ILogger _serilogger;

    public WeatherForecastController(Serilog.ILogger serilogger)
    {
        _serilogger = serilogger;
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _serilogger.Information("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

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
        catch (InvalidOperationException)
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
    public IActionResult CreateCustomer(CustomerDto customerDto)
    {
        // Simulate adding to Db, structured logging (not working with string interpolation)
        // TODO: Log input data
        _serilogger.Information("Writing customer {Lastname}, {Firstname} width id = {id} to DB", 
            customerDto.Lastname, 
            customerDto.Firstname, 
            customerDto.Id);

        return StatusCode(StatusCodes.Status201Created);
    }
}