using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Axpo.PowerTrading.Application.Service
{
	public class PowerPositionService : IPowerPositionService
	{
		private readonly ILogger<PowerPositionService> _logger;
		private readonly IPowerService _powerService;
		private readonly IDateTimeProviderService _dateTimeProviderService;
		public PowerPositionService(
			ILogger<PowerPositionService> logger, 
			IPowerService powerService,
			IDateTimeProviderService dateTimeProviderService)
		{
			_logger = logger;
			_powerService = powerService;
			_dateTimeProviderService = dateTimeProviderService;
		}

		public async Task<List<PowerPosition>> GetPowerPositionsAsync(DateTime date)
		{
			var trades = await _powerService.GetTradesAsync(date);
			return trades
                    .SelectMany(trade => trade.Periods)
                    .GroupBy(period => period.Period)
                    .Select(group => new PowerPosition
                    {
                        Period = _dateTimeProviderService.FormatDateTimePeriodToIso8601(group.Key, date),
                        Volume = group.Sum(p => p.Volume)
                    })
                    .OrderBy(result => result.Period)
                    .ToList();
        }
	}
}
