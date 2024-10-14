using Axpo;
using Axpo.PowerTrading.Application.Service;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {        
        IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
            services =>
            {
                services.AddSingleton<IExportFileService, ExportFileService>();
                services.AddSingleton<IPowerTradeService, PowerTradeService>();
                services.AddSingleton<IPowerService, PowerService>();
                services.AddScoped<IProcessingService, ProcessingService>();
                services.AddHostedService<ConsumerService>();
            }).Build();

        var exportFileService = _host.Services.GetRequiredService<IExportFileService>();
        var tradeService = _host.Services.GetRequiredService<IPowerTradeService>();
        var processingService = _host.Services.GetRequiredService<IProcessingService>();
        var stoppingToken = new CancellationToken();

        await processingService.Process(stoppingToken);

    }
}