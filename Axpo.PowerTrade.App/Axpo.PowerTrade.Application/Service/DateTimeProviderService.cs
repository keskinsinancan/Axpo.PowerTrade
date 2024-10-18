using Axpo.PowerTrading.Application.Service.Interface;
using Axpo.PowerTrading.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Axpo.PowerTrading.Application.Service
{
    public class DateTimeProviderService : IDateTimeProviderService
	{
        private readonly ILogger<DateTimeProviderService> _logger;
		private readonly DateTimeProviderOptions _options;
        private readonly TimeZoneInfo _timeZoneInfo;

		public DateTimeProviderService(ILogger<DateTimeProviderService> logger, IOptions<DateTimeProviderOptions> options)
        {
            _logger = logger;
			_options = options.Value;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZone);
		}

        public string FormatDateTimePeriodToIso8601(int period, DateTime date)
        {
            var updatedDate = date.AddHours(period);
            return updatedDate.ToString(_options.Iso8601Format);
        }
        public DateTime GetUtcDateWithTimeZoneDateOnly(DateTime date)
        {
            var utcDateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _timeZoneInfo);
        }
        public DateTime UtcNowWithTimeZone()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneInfo);
        }
    }
}
