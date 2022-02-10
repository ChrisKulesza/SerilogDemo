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
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
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

/*
 Notizen:
 ✔ Integriert bequem in die appsettings.json (+ automatischer wechsel Dev/Prod)
 ✔ Live config reload
 ✔ Log file management (zb. Log rotate)
 ✔ Gute Dokumentation
 ✔ Auch in statischen methoden bequem zu verwenden
*/