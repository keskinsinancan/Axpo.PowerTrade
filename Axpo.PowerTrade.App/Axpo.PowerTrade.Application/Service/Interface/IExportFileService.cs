namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IExportFileService
    {
        Task<bool> ExportAsync(DateTime date);
    }
}
