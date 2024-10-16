using Axpo.PowerTrading.Application.Constants;
using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Axpo.PowerTrading.Application.Service
{
	public class PowerPositionService : IPowerPositionService
	{
		private readonly ILogger<PowerPositionService> _logger;
		private readonly IPowerService _powerService;
		public PowerPositionService(ILogger<PowerPositionService> logger, IPowerService powerService)
		{
			_logger = logger;
			_powerService = powerService;
		}

		public async Task<List<PowerPosition>> GetPowerPositionsAsync(DateTime date)
		{
			var trades = await _powerService.GetTradesAsync(date);
			return AggregateTrades(trades, date);
		}

		private List<PowerPosition> AggregateTrades(IEnumerable<PowerTrade> trades, DateTime date)
		{
			return
				trades
					.SelectMany(trade => trade.Periods)
					.GroupBy(period => period.Period)
					.Select(group => new PowerPosition
					{
						Period = FormatPeriod(group.Key, date),
						Volume = group.Sum(p => p.Volume)
					})
					.OrderBy(result => result.Period)
					.ToList();
		}

		private string FormatPeriod(int period, DateTime date)
		{
			var sb = new StringBuilder();
			sb.Append(date.ToString("yyyy-MM-dd"));
			sb.Append(FileExportConstants.T);
			sb.Append(ConvertPeriodToHour(period));
			return sb.ToString();	
		}

		private string ConvertPeriodToHour(int period)
		{
			var hour = TimeSpan.FromHours(period);
			return hour.ToString("hh':'mm");
		}
	}
}
