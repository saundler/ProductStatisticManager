using Application.Services;
using ConsoleApp.Extensions;
using ConsoleApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleApp;

internal abstract class Program
{
    private static readonly ILogger<Program> Logger =  LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole() 
            .SetMinimumLevel(LogLevel.Information);
    }).CreateLogger<Program>();
    
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.Sources.Clear();

        var env = builder.Environment;

        builder.Configuration
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),
                    $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Settings{Path.DirectorySeparatorChar}appsettings.json"),
                optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));

        var settings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
        if (settings is null)
        {
            throw new Exception("Configuration not found.");
        }
        builder.Services.AddDataReaders(settings);
        builder.Services.AddDataWriters(settings);
        builder.Services.AddRepositories();
        builder.Services.AddServices(settings);      
        
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        using var host = builder.Build();
        
        var optionMonitor = host.Services.GetRequiredService<IOptionsMonitor<ApplicationSettings>>();
        
        var calculationService = host.Services.GetRequiredService<IDemandProcessingService>();
        
        optionMonitor.OnChange(appSettings =>
            calculationService.MaxDegreeOfParallelismChanged(appSettings.MaxDegreeOfParallelism)); 
        
        ConfigureCancelKeyPressEvent(cancellationTokenSource);
        await host.StartApplication(calculationService, cancellationToken);
    }

    private static void ConfigureCancelKeyPressEvent(CancellationTokenSource cancellationTokenSource)
    {
        Console.CancelKeyPress += (_, args) =>
        {
            Logger.LogInformation("Exiting...");
            args.Cancel = true;
            cancellationTokenSource.Cancel(true);
        };
    }
}