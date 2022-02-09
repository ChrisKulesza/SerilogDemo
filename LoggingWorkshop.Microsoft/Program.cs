using WeatherForecasts.Api;

SharedAspInit.BuildAndRun(args, builder =>
{

});

/*
 Notizen:
 ⚠ Nicht für standalone gedacht
 ✔ Standardmäßig bei Asp.NET Core dabei
 ✔ Plug für allen anderen Logging Libraries (NLog, Serilog, Log4Net)
 ❌ Kein File loggin möglich
 ❌ Wenig konfigurationsmöglichkeiten
*/