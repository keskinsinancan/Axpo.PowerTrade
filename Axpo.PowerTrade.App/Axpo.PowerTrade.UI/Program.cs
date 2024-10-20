﻿using Axpo;
using Axpo.PowerTrading.Application.Service;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Axpo.PowerTrading.Application.Settings;


public class Program
{
    private static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var app = host.Services.GetRequiredService<IProcessingService>();
        var stoppingToken = new CancellationToken();
        await app.Process(stoppingToken);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((context, config) =>
           {
               config.SetBasePath(Directory.GetCurrentDirectory());
               config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
           })
           .ConfigureServices((context, services) =>
           {
               services.Configure<ExportOptions>(context.Configuration.GetSection("ExportOptions"));
			   services.Configure<DateTimeProviderOptions>(context.Configuration.GetSection("DateTimeProviderOptions"));
			   services.AddSingleton<IExportFileService, ExportFileService>();
               services.AddSingleton<IPowerPositionService, PowerPositionService>();
               services.AddSingleton<IPowerService, PowerService>();
               services.AddSingleton<IFileService, FileService>();
               services.AddSingleton<IDateTimeProviderService, DateTimeProviderService>();
               services.AddScoped<IProcessingService, ProcessingService>();
               services.AddHostedService<ConsumerService>();
           });
}