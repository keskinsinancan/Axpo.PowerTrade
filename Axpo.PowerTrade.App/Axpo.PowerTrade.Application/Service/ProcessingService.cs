using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Axpo.PowerTrading.Application.Service
{
	public class ProcessingService : IProcessingService
	{
		private int executionCount = 0;
		private readonly ILogger _logger;
		private readonly IExportFileService _exportFileService;

		public ProcessingService(ILogger<ProcessingService> logger, IExportFileService exportFileService)
		{
			_logger = logger;
			_exportFileService = exportFileService;
		}

		public async Task Process(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				executionCount++;
				_logger.LogInformation("Processing Service is working. Count: {Count}", executionCount);

				var result = await _exportFileService.ExportAsync(DateTime.Now);
				//todo : retry mechanism 


				await Task.Delay(10000, stoppingToken);
			}
		}
	}
}
