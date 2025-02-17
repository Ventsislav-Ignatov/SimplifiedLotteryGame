using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimplifiedLotteryGame.ConsoleApp;
using SimplifiedLotteryGame.Data;
using SimplifiedLotteryGame.Services;
using Microsoft.Extensions.Logging;
using SimplifiedLotteryGame.Common.Services;

IHost _host = Host.CreateDefaultBuilder()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Error);
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration config = hostContext.Configuration;

        string connectionString = config.GetConnectionString("Database");

        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        // Register application services
        services.AddScoped<IApplication, Application>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IConsoleReader, ConsoleReader>();
        services.AddSingleton<IRandomNumberGeneratorService, RandomNumberGeneratorService>();
        services.AddSingleton<IPlayerFactoryService, PlayerFactoryService>();
    })
    .Build();

var app = _host.Services.GetRequiredService<IApplication>();
await app.Run();
