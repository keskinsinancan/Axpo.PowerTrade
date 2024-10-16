using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Axpo.PowerTrading.Application.Service.Interface;

namespace Axpo.PowerTrading.Application.Service
{
	public class ConsumerService : BackgroundService
	{
		private readonly ILogger<ConsumerService> _logger;

		public ConsumerService(IServiceProvider services, ILogger<ConsumerService> logger)
		{
			Services = services;
			_logger = logger;
		}

		public IServiceProvider Services { get; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consumer Service running.");

			await DoWork(stoppingToken);
		}

		private async Task DoWork(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consumer Service is working.");

			using (var scope = Services.CreateScope())
			{
				var processingService = scope.ServiceProvider.GetRequiredService<IProcessingService>();
				await processingService.Process(stoppingToken);
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consumer Service is stopping.");

			await base.StopAsync(stoppingToken);
		}
	}
}
