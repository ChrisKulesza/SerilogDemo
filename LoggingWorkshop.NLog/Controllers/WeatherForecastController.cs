using Microsoft.AspNetCore.Mvc;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private static readonly NLog.Logger CustomerLogger = NLog.LogManager.GetLogger(nameof(CustomerDto));

    public WeatherForecastController()
    {

    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        Logger.Info("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

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
        Logger.Trace("LVL: Trace");
        Logger.Debug("LVL: Debug");
        Logger.Info("LVL: Info");
        Logger.Warn("LVL: Warning");
        Logger.Error("LVL: Error");
        Logger.Fatal("LVL: Critical");

        return Ok();
    }

    // Logging categories
    [HttpGet("categories")]
    public IActionResult GetLogCategories()
    {
        Logger.Info("From WeatherForecastController ({logger})", nameof(Logger));
        CustomerLogger.Info("From CustomerDto ({logger})", nameof(CustomerLogger));

        return Ok();
    }

    // Log scope
    [HttpGet("scopes")]
    public IActionResult GetLogScopes([FromQuery] string? sort)
    {
        using (NLog.MappedDiagnosticsLogicalContext.SetScoped("query", sort))
        {
            Logger.Info("From Scope");
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
            Logger.Warn(ioex, "Operation did not complete successfully");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("exception2")]
    public IActionResult HandleException2()
    {
        // NLog spezifische hilfsfunktion
        Logger.Swallow(() =>
        {
            // Wörk wörk
            throw new InvalidOperationException("Something bad happened");
            // Other wörk
        });

        return Ok();
    }

    // Endpoint that throws a unhandled exception
    [HttpGet("unhandled-exception")]
    public IActionResult UnhandledException() => throw new InvalidOperationException("Something bad happend.");

    // POST
    [HttpPost("customers")]
    public IActionResult CreateCustomer(CustomerDto customer)
    {
        Logger.Info("Got Customer create request: {customer}", customer);

        return StatusCode(StatusCodes.Status201Created);
    }

    // Andere NLog spezifischen features

    private static void Helper()
    {
        Logger.ConditionalTrace("I am speed");
    }
}