using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

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
        _serilogger.Verbose("LVL: Verbose");
        _serilogger.Debug("LVL: Debug");
        _serilogger.Information("LVL: Information");
        _serilogger.Warning("LVL: Warning");
        _serilogger.Error("LVL: Error");
        _serilogger.Fatal("LVL: Fatal");

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
            _serilogger.Warning(ioex, "Operation did not complete successfully");

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
        _serilogger.Information("Writing customer {Lastname}, {Firstname} width id = {Id} to DB",
            customerDto.Lastname, 
            customerDto.Firstname, 
            customerDto.Id);

        return StatusCode(StatusCodes.Status201Created);
    }
}