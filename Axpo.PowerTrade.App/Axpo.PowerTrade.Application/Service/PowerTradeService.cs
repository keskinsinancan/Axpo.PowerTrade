using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Axpo.PowerTrading.Application.Service
{
    public class PowerTradeService : IPowerTradeService
    {
        private readonly ILogger<PowerTradeService> _logger;
        private readonly IPowerService _powerService;
        public PowerTradeService(ILogger<PowerTradeService> logger, IPowerService powerService)
        {
            _logger = logger;
            _powerService = powerService;
        }
        public IEnumerable<PowerTrade> GetPowerTrades(DateTime date)
        {
            return _powerService.GetTrades(date);
        }

        public async Task<List<AggregatedPeriod>> GetPowerTradesAsync(DateTime date)
        {
            var trades = await _powerService.GetTradesAsync(date);
            var aggTrades = AggregateTrades(trades);
            return aggTrades;
        }

        private List<AggregatedPeriod> AggregateTrades(IEnumerable<PowerTrade> trades)
        {
            var aggTrades = new List<AggregatedPeriod>();

            var aggregatedVolumes = trades
                .SelectMany(trade => trade.Periods)
                .GroupBy(period => period.Period)
                .Select(group => new
                {
                    Period = group.Key,
                    TotalVolume = group.Sum(p => p.Volume)
                })
                .OrderBy(result => result.Period)
                .ToList();

            aggTrades.AddRange(
                aggregatedVolumes
                .Select(vol => new AggregatedPeriod
                {
                    Period = vol.Period,
                    Volume = vol.TotalVolume
                })
             );

            return aggTrades;
        }
    }
}
