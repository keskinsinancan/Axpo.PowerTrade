namespace Axpo.PowerTrading.Application.Settings
{
    public class ExportOptions
    {
        public string? ExportFolderPath { get; set; }
        public int ExportIntervalInMiliseconds { get; set; }
    }

    public class DateTimeProviderOptions
    {
        public string? TimeZone { get; set; }
        public string? Iso8601Format { get; set; }
    }
}
