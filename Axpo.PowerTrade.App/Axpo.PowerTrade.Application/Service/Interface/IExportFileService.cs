namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IExportFileService
    {
        Task<bool> ExportToCsvAsync(DateTime date);
    }
}
