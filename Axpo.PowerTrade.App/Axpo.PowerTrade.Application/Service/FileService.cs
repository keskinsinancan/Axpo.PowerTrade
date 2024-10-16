using Axpo.PowerTrading.Application.Constants;
using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service.Interface;
using System.Text;

namespace Axpo.PowerTrading.Application.Service
{
    public class FileService : IFileService
    {
        public async Task ExportToCsv(List<PowerPosition> periods, string path)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine($"{FileExportConstants.Datetime}, {FileExportConstants.Volume}");

            foreach (var period in periods)
            {
                csvContent.AppendLine($"{period.Period},{period.Volume}");
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(csvContent.ToString());
            }
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
