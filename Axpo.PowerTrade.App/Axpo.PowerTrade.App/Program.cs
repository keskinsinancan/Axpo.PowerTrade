using Axpo;
using Axpo.PowerTrading.Application.Service;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;

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
        }).Build();

        var exportFileService = _host.Services.GetRequiredService<IExportFileService>();
        var tradeService = _host.Services.GetRequiredService<IPowerTradeService>();

        var res = await exportFileService.ExportAsync(DateTime.Now);
    }
}