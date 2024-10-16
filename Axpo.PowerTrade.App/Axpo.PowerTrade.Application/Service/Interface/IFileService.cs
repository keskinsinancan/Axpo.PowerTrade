using Axpo.PowerTrading.Application.Models;

namespace Axpo.PowerTrading.Application.Service.Interface
{
    public interface IFileService
    {
        Task ExportToCsv(List<PowerPosition> periods, string path);
        void CreateDirectory(string path);
        bool FileExists(string path);
    }
}
