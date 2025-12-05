using IceCity_Data_Management_System;
using IceCity_Data_Management_System.Configuration;
using IceCity_Data_Management_System.Persistence.Seed;
using IceCity_Data_Management_System.UIHelper;
using Microsoft.Extensions.Hosting;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    var logsPath = Path.Combine(AppContext.BaseDirectory, "Logs");
    if (!Directory.Exists(logsPath))
        Directory.CreateDirectory(logsPath);

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        })
        .ConfigureServices((context, services) =>
        {
            services.AddDependencies(context.Configuration);
            var configSingleton = AppConfig.GetInstance(context.Configuration);
            services.AddSingleton(configSingleton);
        })
        .Build();

    using (var scope = host.Services.CreateScope())
    {
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<ApplicationDbContext>();
        await ApplicationDbInitializer.SeedAsync(context);

        await ConsoleFormatter.RunConsoleMenuAsync(provider);
    }

    ConsoleFormatter.Success("Exiting IceCity...");
    Log.CloseAndFlush();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The IceCity backend terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}






