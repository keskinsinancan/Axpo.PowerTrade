using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Axpo.PowerTrading.Application.Service
{
    public class ExportFileService : IExportFileService
    {
        private readonly ILogger<ExportFileService> _logger;
        private readonly IPowerTradeService _powerTradeService;
        public ExportFileService(ILogger<ExportFileService> logger, IPowerTradeService powerTradeService)
        {
            _logger = logger;
            _powerTradeService = powerTradeService;
        }
        public async Task<bool> ExportAsync(DateTime date)
        {
            try
            {
                var dayEhead = CalculateDayEhead(date);
                _logger.LogInformation($"Attemping retrieve data for the day {date.Date}");
                var aggregatedPeriods = await _powerTradeService.GetPowerTradesAsync(dayEhead);
                //todo add nullcheck
                var path = await ExportTradesToFile(aggregatedPeriods);
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

        private async Task<string> ExportTradesToFile(List<AggregatedPeriod> periods)
        {
            return "asda";
        }
    }
}
