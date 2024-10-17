using Axpo.PowerTrading.Application.Service.Interface;
using System;

namespace Axpo.PowerTrading.Application.Service
{
    public class DateTimeProviderService : IDateTimeProviderService
	{
        private static readonly TimeZoneInfo _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        
        public string FormatDateTimePeriodToIso8601(int period, DateTime date)
        {
            var updatedDate = date.AddHours(period);
            return updatedDate.ToString("yyyy-MM-ddTHH:mmZ");
        }
        public DateTime GetUtcDateTimeWithTimeZone(DateTime date)
        {
            var utcDateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _timeZoneInfo);
        }
        public DateTime UtcNowWithTimeZone()
        {
            var date = DateTime.UtcNow.AddHours(1);
            return TimeZoneInfo.ConvertTimeFromUtc(date, _timeZoneInfo);
        }
    }
}
