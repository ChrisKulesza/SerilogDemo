using Microsoft.AspNetCore.Mvc;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ILogger<CustomerDto> _customerLogger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ILogger<CustomerDto> customerLogger)
    {
        _logger = logger;
        _customerLogger = customerLogger;
    }

    // Simple Get
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Entered {Controller}.{Methode}", nameof(WeatherForecastController), nameof(Get));

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
        _logger.LogTrace("LVL: Trace");
        _logger.LogDebug("LVL: Debug");
        _logger.LogInformation("LVL: Info");
        _logger.LogWarning("LVL: Warning");
        _logger.LogError("LVL: Error");
        _logger.LogCritical("LVL: Critical");

        return Ok();
    }

    // Logging categories
    [HttpGet("categories")]
    public IActionResult GetLogCategories()
    {
        _logger.LogInformation("From WeatherForecastController ({logger})", nameof(_logger));
        _customerLogger.LogInformation("From CustomerDto ({logger})", nameof(_customerLogger));

        return Ok();
    }

    // Log scope
    [HttpGet("scopes")]
    public IActionResult GetLogScopes([FromQuery] string? sort)
    {
        using (_logger.BeginScope("Query Scope {query}", sort))
        {
            _logger.LogInformation("From Scope");
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
            _logger.LogWarning(ioex, "Operation did not complete successfully");

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
        _logger.LogInformation("Got Customer create request: {customer}", customer);

        return StatusCode(StatusCodes.Status201Created);
    }
}