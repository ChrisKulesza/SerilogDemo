using log4net.Config;
using WeatherForecasts.Api;

XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

SharedAspInit.BuildAndRun(args, builder =>
{
    builder.Logging.AddLog4Net();
});

/*
 Notizen:
 ✔ Global/Thread/Async context mapping
 ❌ Normales Logging hat kein overload für Exception + formatierte Nachricht
 ❌ Keine 'named' parameter für format strings
 ❌ Kein 'Trace' Level
 ❌ Config Live Reload nach dokumentation möglich, habs aber nich zum laufen bekommen
*/