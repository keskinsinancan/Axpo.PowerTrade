using Axpo.PowerTrading.Application.Models;

namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IPowerTradeService
    {
        IEnumerable<PowerTrade> GetPowerTrades(DateTime date);
        Task<List<AggregatedPeriod>> GetPowerTradesAsync(DateTime date);
    }
}
