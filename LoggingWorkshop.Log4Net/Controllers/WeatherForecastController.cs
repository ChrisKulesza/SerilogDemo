using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecasts.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly ILog _logger = LogManager.GetLogger(typeof(WeatherForecastController));
    private static readonly ILog _customerLogger = LogManager.GetLogger(typeof(CustomerDto));

    public WeatherForecastController()
    {
    }

    // GET Enpoint
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.InfoFormat("Entered {0}.{1}", nameof(WeatherForecastController), nameof(Get));

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
        //_logger.Trace("LVL: Trace"); // Kein trace
        _logger.Debug("LVL: Debug");
        _logger.Info("LVL: Info");
        _logger.Warn("LVL: Warning");
        _logger.Error("LVL: Error");
        _logger.Fatal("LVL: Critical");

        return Ok();
    }

    // Logging categories
    [HttpGet("categories")]
    public IActionResult GetLogCategories()
    {
        _logger.InfoFormat("From WeatherForecastController ({0})", nameof(_logger));
        _customerLogger.InfoFormat("From CustomerDto ({0})", nameof(_customerLogger));

        return Ok();
    }

    // Log scope
    [HttpGet("scopes")]
    public IActionResult GetLogScopes([FromQuery] string? sort)
    {
        //GlobalContext
        //LogicalThreadContext
        using (ThreadContext.Stacks["query"].Push(sort))
        {
            _logger.Info("From Scope");
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
            _logger.Warn("Operation did not complete successfully", ioex);

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
        _logger.InfoFormat("Got Customer create request: {0}", customer);

        return StatusCode(StatusCodes.Status201Created);
    }
}