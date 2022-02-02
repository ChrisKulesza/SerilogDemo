namespace WeatherForecasts.Api;

public class SharedAspInit
{
    public static void BuildAndRun(string[] args, Action<WebApplicationBuilder> buildAction)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        buildAction(builder);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    public static void GlobalHandler() {
        //AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        //{
        //    Log.Logger.Fatal(e.ExceptionObject as Exception, "Fatal error");
        //    Log.CloseAndFlush();
        //};
    }
}
