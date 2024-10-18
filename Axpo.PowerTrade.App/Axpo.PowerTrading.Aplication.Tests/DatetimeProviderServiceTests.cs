using Axpo.PowerTrading.Application.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Axpo.PowerTrading.Application.Settings;
using Moq;
using Axpo.PowerTrading.Application.Service.Interface;

namespace Axpo.PowerTrading.Application.Tests
{
    public class DateTimeProviderServiceTests
    {
		private readonly Mock<ILogger<DateTimeProviderService>> _mockLogger;
		private readonly DateTimeProviderService _dateTimeProviderService;

		public DateTimeProviderServiceTests()
        {
			_mockLogger = new Mock<ILogger<DateTimeProviderService>>();
			_dateTimeProviderService = new DateTimeProviderService
				(
					_mockLogger.Object,
					Options.Create(new DateTimeProviderOptions
					{
						Iso8601Format = "yyyy-MM-ddTHH:mmZ",
						TimeZone = "Europe/Berlin"
					})
				);
        }

		[Fact]
		public void FormatDateTimePeriodToIso8601_ValidPeriod_ReturnsCorrectIso8601Format()
		{
			var date = new DateTime(2024, 10, 17, 12, 0, 0, DateTimeKind.Utc);
			int period = 2;

			var result = _dateTimeProviderService.FormatDateTimePeriodToIso8601(period, date);

			var expectedDate = date.AddHours(period).ToString("yyyy-MM-ddTHH:mmZ");
			Assert.Equal(expectedDate, result);
		}

		[Fact]
		public void GetUtcDateTimeWithTimeZone_ConvertsToBerlinTimeZone_CorrectlyAdjustsForDST()
		{
			var utcDate = new DateTime(2024, 3, 31, 0, 0, 0, DateTimeKind.Utc);

			var result = _dateTimeProviderService.GetUtcDateWithTimeZoneDateOnly(utcDate);

			var berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
			var expected = TimeZoneInfo.ConvertTimeFromUtc(utcDate, berlinTimeZone);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void UtcNowWithTimeZone_ReturnsCurrentBerlinTime()
		{
			var utcNow = DateTime.UtcNow;

			var result = _dateTimeProviderService.UtcNowWithTimeZone();

			var berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
			var expected = TimeZoneInfo.ConvertTimeFromUtc(utcNow, berlinTimeZone);
			Assert.Equal(expected.Date, result.Date);
		}
	}
}
