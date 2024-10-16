using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using Microsoft.Extensions.Options;
using Axpo.PowerTrading.Application.Settings;

namespace Axpo.PowerTrading.Aplication.Tests
{
    public class ExportFileServiceTests
    {
        private readonly Mock<IPowerPositionService> _mockPowerTradeService;
        private readonly Mock<IFileService> _mockFileService;
        private readonly Mock<IDateTimeProviderService> _mockDateTimeProviderService;
        private readonly Mock<ILogger<ExportFileService>> _mockLogger;
        private readonly ExportFileService _exportFileService;

        public ExportFileServiceTests()
        {
            _mockPowerTradeService = new Mock<IPowerPositionService>();
            _mockFileService = new Mock<IFileService>();
            _mockDateTimeProviderService = new Mock<IDateTimeProviderService>();
            _mockLogger = new Mock<ILogger<ExportFileService>>();

            _exportFileService = new ExportFileService(
                _mockLogger.Object,
                _mockPowerTradeService.Object,
                _mockFileService.Object,
                _mockDateTimeProviderService.Object,
                Options.Create(new ExportOptions { ExportFolderPath = "/test/path" })
            );
        }

        [Fact]
        public async Task ExportToCsvAsync_SuccessfulExport_ReturnsTrue()
        {
            var testDate = DateTime.Now;
            _mockDateTimeProviderService.Setup(d => d.UtcNowWithTimeZone()).Returns(DateTime.UtcNow);
            _mockPowerTradeService.Setup(s => s.GetPowerPositionsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerPosition>
                    { new PowerPosition
                        {
                        Period = "1",
                        Volume = 100 }
                });

            var result = await _exportFileService.ExportToCsvAsync(testDate);

            Assert.True(result);
            _mockFileService.Verify(f =>
                f.ExportToCsv(It.IsAny<List<PowerPosition>>(),
                It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ExportToCsvAsync_WhenIOExceptionOccurs_ReturnsFalse()
        {
            var testDate = DateTime.Now;
            _mockDateTimeProviderService.Setup(d => d.UtcNowWithTimeZone()).Returns(DateTime.UtcNow);

            _mockPowerTradeService.Setup(s => s.GetPowerPositionsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerPosition>
                    { new PowerPosition {
                        Period = "1",
                        Volume = 100 }
                });

            _mockFileService.Setup(f => f.ExportToCsv(It.IsAny<List<PowerPosition>>(), It.IsAny<string>()))
                .ThrowsAsync(new IOException("Test I/O exception"));

            var result = await _exportFileService.ExportToCsvAsync(testDate);

            Assert.False(result);
        }

        [Fact]
        public async Task ExportToCsvAsync_WhenGeneralExceptionOccurs_ReturnsFalse()
        {
            var testDate = DateTime.Now;
            _mockDateTimeProviderService.Setup(d => d.UtcNowWithTimeZone()).Returns(DateTime.UtcNow);

            _mockPowerTradeService.Setup(s => s.GetPowerPositionsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerPosition>
                    { new PowerPosition
                        { Period = "1",
                        Volume = 100 }
                });

            _mockFileService.Setup(f => f.ExportToCsv(It.IsAny<List<PowerPosition>>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test general exception"));

            var result = await _exportFileService.ExportToCsvAsync(testDate);
            Assert.False(result);
        }

        [Fact]
        public async Task ExportToCsvAsync_WhenEmptyPowerPositions_ReturnsFalse()
        {
            var testDate = DateTime.Now;
            _mockDateTimeProviderService.Setup(d => d.UtcNowWithTimeZone()).Returns(DateTime.UtcNow);

            _mockPowerTradeService.Setup(s => s.GetPowerPositionsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerPosition>());

            var result = await _exportFileService.ExportToCsvAsync(testDate);
            Assert.False(result);
        }

        [Fact]
        public async Task ExportToCsvAsync_WhenInvalidExportPath_ReturnsFalse()
        {
            var testDate = DateTime.Now;
            _mockDateTimeProviderService.Setup(d => d.UtcNowWithTimeZone()).Returns(DateTime.UtcNow);

            var invalidOptions = Options.Create(new ExportOptions { ExportFolderPath = null });
            var exportFileServiceWithInvalidOptions = 
                new ExportFileService(
                        _mockLogger.Object, 
                        _mockPowerTradeService.Object,
                        _mockFileService.Object,
                        _mockDateTimeProviderService.Object, invalidOptions
                        );

            var result = await exportFileServiceWithInvalidOptions.ExportToCsvAsync(testDate);

            Assert.False(result); 
        }
    }
}