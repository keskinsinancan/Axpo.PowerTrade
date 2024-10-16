using Axpo.PowerTrading.Application.Service.Interface;

namespace Axpo.PowerTrading.Application.Service
{
    public class DateTimeProviderService : IDateTimeProviderService
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
