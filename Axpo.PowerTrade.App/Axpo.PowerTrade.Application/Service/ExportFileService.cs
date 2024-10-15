using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Axpo.PowerTrading.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

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
            _logger.LogInformation($"Reading options Name :{_options.Option1} {_options.Option2}");

            try
            {
                var dayEhead = CalculateDayEhead(date);
                _logger.LogInformation($"Attemping retrieve data for the day {date.Date}");
                var aggregatedTrades = await _powerTradeService.GetPowerTradesAsync(dayEhead);
                var path = BuildPath(date);
                await ExportToCsv(aggregatedTrades, path);
                if (Path.Exists(path))
                {
                    _logger.LogInformation($"File trades are exported successfully for the date {date.Date}");
                    return true;
                }
                _logger.LogInformation("File export was not successfull");
                return false;
            }
            catch (Exception ex) {
                _logger.LogInformation($"Failed to export {ex}");
                return false;
            }
        }

        private DateTime CalculateDayEhead(DateTime date) => date.AddDays(1);

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

        private string BuildPath(DateTime date)
        {
            return "C:/Users/keski/Desktop/PowerPositions/output.csv";
        }
    }
}
