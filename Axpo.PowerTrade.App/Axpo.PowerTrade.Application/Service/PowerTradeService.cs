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

        public async Task<List<AggregatePeriod>> GetPowerTradesAsync(DateTime date)
        {
            var trades = await _powerService.GetTradesAsync(date);
            return AggregateTrades(trades);
        }

        private List<AggregatePeriod> AggregateTrades(IEnumerable<PowerTrade> trades)
        {
            return
                trades
                    .SelectMany(trade => trade.Periods)
                    .GroupBy(period => period.Period)
                    .Select(group => new AggregatePeriod
                    {
                        Period = group.Key,
                        Volume = group.Sum(p => p.Volume)
                    })
                    .OrderBy(result => result.Period)
                    .ToList();
        }
    }
}
