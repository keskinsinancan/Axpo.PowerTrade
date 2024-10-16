using Axpo.PowerTrading.Application.Service.Interface;
using Axpo.PowerTrading.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Axpo.PowerTrading.Application.Constants;

public class ExportFileService : IExportFileService
{
    private readonly ILogger<ExportFileService> _logger;
    private readonly IPowerPositionService _powerTradeService;
    private readonly IFileService _fileService;
    private readonly IDateTimeProviderService _dateTimeProviderService;
    private readonly ExportOptions _options;

    public ExportFileService(
        ILogger<ExportFileService> logger,
        IPowerPositionService powerPositionService,
        IFileService fileService,
        IDateTimeProviderService dateTimeProviderService,
        IOptions<ExportOptions> options)
    {
        _logger = logger;
        _powerTradeService = powerPositionService;
        _fileService = fileService;
        _dateTimeProviderService = dateTimeProviderService;
        _options = options.Value;
    }

    public async Task<bool> ExportToCsvAsync(DateTime date)
    {
        var dayEhead = date.AddDays(1);
        _logger.LogInformation($"Attempting to retrieve data for the day {dayEhead.ToShortDateString()}");

        try
        {
            var powerPositions = await _powerTradeService.GetPowerPositionsAsync(dayEhead);
            
            if (!powerPositions.Any())
            {
                _logger.LogError($"Positions could not be retrieved for the date {dayEhead.ToShortDateString()}");
                return false;
            }

            var path = BuildExportPath(dayEhead);
            await _fileService.ExportToCsv(powerPositions, path);
            _logger.LogInformation($"File successfully exported to {path} for the date {dayEhead.ToShortDateString()}");
            return true;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, $"I/O error during file export for {dayEhead.ToShortDateString()}: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred during file export for {dayEhead.ToShortDateString()}: {ex.Message}");
        }

        _logger.LogWarning($"File export failed for {dayEhead.ToShortDateString()}.");
        return false;
    }

    private string BuildExportPath(DateTime date)
    {
        _fileService.CreateDirectory(_options.ExportFolderPath);

        var sb = new StringBuilder();
        sb.Append(_options.ExportFolderPath);
        sb.Append(FileExportConstants.PowerPosition);
        sb.Append('_');
        sb.Append(date.ToString("yyyyMMdd"));
        sb.Append('_');
        sb.Append(_dateTimeProviderService.UtcNowWithTimeZone().ToString("yyyyMMddHHmm"));
        sb.Append(".csv");
        return sb.ToString();
    }
}
