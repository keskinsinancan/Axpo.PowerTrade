using Axpo.PowerTrading.Application.Service.Interface;

namespace Axpo.PowerTrading.Application.Service
{
    public class DateTimeProviderService : IDateTimeProviderService
    {
        public string FormatDateTimePeriodToIso8601(int period, DateTime date)
        {
            var updatedDate = date.AddHours(period + 1);
            return updatedDate.ToString("yyyy-MM-ddTHH:mmZ");
        }

        public DateTime GetUtcDateTimeWithTimeZone(DateTime date)
        {
            //UTC + 1 to ensure Berlin Timezone as default
            return new DateTime(date.Year, date.Month, date.Day, 1, 0, 0, DateTimeKind.Utc);
        }

        public DateTime UtcNowWithTimeZone()
        {
            //UTC + 1 to ensure Berlin Timezone as default
            var date = DateTime.UtcNow.AddHours(1);
            return date;
        }
    }
}
