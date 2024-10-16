using Axpo.PowerTrading.Application.Models;

namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IPowerPositionService
    {
        Task<List<PowerPosition>> GetPowerPositionsAsync(DateTime date);
    }
}
