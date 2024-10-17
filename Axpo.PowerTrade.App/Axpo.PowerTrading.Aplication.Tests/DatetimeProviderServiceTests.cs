using Axpo.PowerTrading.Application.Service;

namespace Axpo.PowerTrading.Application.Tests
{
    public class DateTimeProviderServiceTests
    {
        private readonly DateTimeProviderService _dateTimeProviderService;

        public DateTimeProviderServiceTests()
        {
            _dateTimeProviderService = new DateTimeProviderService();
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

			var result = _dateTimeProviderService.GetUtcDateTimeWithTimeZone(utcDate);

			TimeZoneInfo berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
			var expected = TimeZoneInfo.ConvertTimeFromUtc(utcDate, berlinTimeZone);
			Assert.Equal(expected, result);
		}

		[Fact]
		public void UtcNowWithTimeZone_ReturnsCurrentBerlinTime()
		{
			var utcNow = new DateTime(2024, 10, 17, 12, 0, 0, DateTimeKind.Utc);

			var result = _dateTimeProviderService.UtcNowWithTimeZone();

			TimeZoneInfo berlinTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
			var expected = TimeZoneInfo.ConvertTimeFromUtc(utcNow.AddHours(1), berlinTimeZone);
			Assert.Equal(expected.Date, result.Date);
		}
	}
}
