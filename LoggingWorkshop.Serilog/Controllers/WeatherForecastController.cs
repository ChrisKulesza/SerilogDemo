using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly Serilog.ILogger _logger = Log.ForContext<WeatherForecastController>();
    private static readonly Serilog.ILogger CustomerLogger = Log.ForContext<CustomerDto>();

    public WeatherForecastController()
    {
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.Information("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Weather.Summaries[Random.Shared.Next(Weather.Summaries.Length)]
        })
        .ToArray();
    }

    // Checking out logging levels
    [HttpGet("levels")]
    public IActionResult GetLogLevels()
    {
        _logger.Verbose("LVL: Trace");
        _logger.Debug("LVL: Debug");
        _logger.Information("LVL: Info");
        _logger.Warning("LVL: Warning");
        _logger.Error("LVL: Error");
        _logger.Fatal("LVL: Critical");

        return Ok();
    }

    [HttpGet("categories")]
    public IActionResult GetLogCategories()
    {
        _logger.Information("From WeatherForecastController ({logger})", nameof(_logger));
        CustomerLogger.Information("From CustomerDto ({logger})", nameof(CustomerLogger));

        return Ok();
    }

    [HttpGet("scopes")]
    public IActionResult GetLogScopes([FromQuery] string? sort)
    {
        using (LogContext.PushProperty("query", sort))
        {
            _logger.Information("From Scope");
        }

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
            _logger.Warning(ioex, "Operation did not complete successfully");

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
        _logger.Information("Got Customer create request: {customer}", customerDto);

        return StatusCode(StatusCodes.Status201Created);
    }
}