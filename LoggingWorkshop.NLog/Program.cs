using WeatherForecasts.Api;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("init main");

try
{
    SharedAspInit.BuildAndRun(args, builder =>
    {
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        builder.Host.UseNLog();
    });
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

/*
 Notizen:
 ✔ Vielseitiges target + rule system
 ✔ Live config reload
 ✔ Gute Dokumentation
 ✔ Log file management (zb. Log rotate)
 ✔ Auch in statischen methoden bequem zu verwenden
 ✔ Global/Thread/Async context mapping
 ❌ Aufwändig gute Layouts zu schreiben / Können schnell kompliziert werden
*/