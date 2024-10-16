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
        public void FormatDateTimePeriodToIso8601_ValidInput_ReturnsCorrectFormat()
        {
            int period = 5;
            DateTime date = new DateTime(2024, 10, 17, 0, 0, 0, DateTimeKind.Utc);

            string result = _dateTimeProviderService.FormatDateTimePeriodToIso8601(period, date);

            Assert.Equal("2024-10-17T06:00Z", result);
        }

        [Fact]
        public void GetUtcDateTimeWithTimeZone_ValidInput_ReturnsCorrectUtcDateTime()
        {
            DateTime inputDate = new DateTime(2024, 10, 17, 0, 0, 0, DateTimeKind.Utc);

            DateTime result = _dateTimeProviderService.GetUtcDateTimeWithTimeZone(inputDate);

            Assert.Equal(new DateTime(2024, 10, 17, 1, 0, 0, DateTimeKind.Utc), result);
        }

        [Fact]
        public void UtcNowWithTimeZone_ReturnsCorrectUtcDateTime()
        {
            DateTime result = _dateTimeProviderService.UtcNowWithTimeZone();

            Assert.True(result >= DateTime.UtcNow.AddHours(1).AddMinutes(-1) &&
                        result <= DateTime.UtcNow.AddHours(1).AddMinutes(1));
        }
    }
}
