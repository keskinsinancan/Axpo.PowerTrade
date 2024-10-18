using Axpo.PowerTrading.Application.Service.Interface;
using Axpo.PowerTrading.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Axpo.PowerTrading.Application.Service
{
	public class ProcessingService : IProcessingService
	{
		private int executionCount = 0;
		private readonly ILogger _logger;
		private readonly IExportFileService _exportFileService;
		private readonly IDateTimeProviderService _dateTimeProviderService;
		private readonly ExportOptions _options;

		public ProcessingService(
			ILogger<ProcessingService> logger, 
			IExportFileService exportFileService,
			IDateTimeProviderService dateTimeProviderService,
			IOptions<ExportOptions> options)
		{
			_logger = logger;
			_exportFileService = exportFileService;
			_dateTimeProviderService = dateTimeProviderService;
			_options = options.Value;
		}

		public async Task Process(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var result = false;
				var retryExportCount = 0;
				executionCount++;
				_logger.LogInformation($"Processing Service is working. Count: {executionCount}");

				while (!result)
				{
					retryExportCount++;
					_logger.LogInformation($"Exporting csv attemp number {retryExportCount} ");
					var utcNowDateOnly = _dateTimeProviderService.GetUtcDateWithTimeZoneDateOnly(DateTime.UtcNow);
                    result = await _exportFileService.ExportToCsvAsync(utcNowDateOnly);
                }
				
				await Task.Delay(_options.ExportIntervalInMiliseconds, stoppingToken);
			}
		}
	}
}
