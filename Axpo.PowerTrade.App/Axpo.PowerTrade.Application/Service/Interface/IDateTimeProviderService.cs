namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IDateTimeProviderService
    {
        DateTime UtcNowWithTimeZone();
        string FormatDateTimePeriodToIso8601(int period, DateTime date);
        DateTime GetUtcDateTimeWithTimeZone(DateTime date);
    }
}
