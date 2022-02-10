using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using WeatherForecasts.Api;

// Setup serilog in a two-step process. First, we configure basic logging
// to be able to log errors during ASP.NET Core startup. Later, we read
// log settings from appsettings.json. Read more at
// https://github.com/serilog/serilog-aspnetcore#two-stage-initialization.
// General information about serilog can be found at
// https://serilog.net/
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: SystemConsoleTheme.Colored)
    .CreateLogger();

try
{
    Log.Information("Starting the web host");

    SharedAspInit.BuildAndRun(args, builder =>
    {
        // load serilog.json to IConfiguration
        var jsonReload = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            // reloadOnChange will allow you to auto reload the minimum level and level switches
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(jsonReload)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    },
    app =>
    {
        app.UseSerilogRequestLogging();
    });
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectectly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;