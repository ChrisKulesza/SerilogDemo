using log4net.Config;
using WeatherForecasts.Api;

XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

SharedAspInit.BuildAndRun(args, builder =>
{
    builder.Logging.AddLog4Net();
});