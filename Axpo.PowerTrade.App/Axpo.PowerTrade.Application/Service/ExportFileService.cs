using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Axpo.PowerTrading.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Axpo.PowerTrading.Application.Constants;

namespace Axpo.PowerTrading.Application.Service
{
	public class ExportFileService : IExportFileService
	{
		private readonly ILogger<ExportFileService> _logger;
		private readonly IPowerTradeService _powerTradeService;
		private readonly ExportOptions _options;

		public ExportFileService(
			ILogger<ExportFileService> logger,
			IPowerTradeService powerTradeService,
			IOptions<ExportOptions> options)
		{
			_logger = logger;
			_powerTradeService = powerTradeService;
			_options = options.Value;
		}
		public async Task<bool> ExportToCsvAsync(DateTime date)
		{
			try
			{
				var dayEhead = date.AddDays(1);
				_logger.LogInformation($"Attemping retrieve data for the day {dayEhead.ToShortDateString()}");

				var aggregatedTrades = await _powerTradeService.GetPowerTradesAsync(dayEhead);

				var path = BuildExportPath(dayEhead);

				await ExportToCsv(aggregatedTrades, path);
				if (Path.Exists(path))
				{
					_logger.LogInformation($"File trades are exported successfully for the date {dayEhead.Date}");
					return true;
				}
				_logger.LogInformation("File export was not successfull");
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Failed to export {ex}");
				return false;
			}
		}

		private async Task ExportToCsv(List<AggregatePeriod> periods, string path)
		{
			var csvContent = new StringBuilder();
			csvContent.AppendLine("Period,Volume");

			foreach (var period in periods)
			{
				csvContent.AppendLine($"{period.Period},{period.Volume}");
			}

			using (StreamWriter writer = new StreamWriter(path, false))
			{
				await writer.WriteAsync(csvContent.ToString());
			}
		}

		private string BuildExportPath(DateTime date)
		{
			Directory.CreateDirectory(_options.ExportFolderPath);

			var sb = new StringBuilder();
			sb.Append(_options.ExportFolderPath);
			sb.Append(FileExportConstants.PowerPosition);
			sb.Append('_');
			sb.Append(date.ToString("yyyyMMdd"));
			sb.Append('_');
			sb.Append(DateTime.UtcNow.ToString("yyyyMMddhmm"));
			return sb.ToString();
		}
	}
}
