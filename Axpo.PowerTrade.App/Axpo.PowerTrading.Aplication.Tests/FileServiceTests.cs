using Axpo.PowerTrading.Application.Models;
using Axpo.PowerTrading.Application.Service;

namespace Axpo.PowerTrading.Application.Tests
{
    public class FileServiceTests
    {
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public async Task ExportToCsv_ValidInput_CreatesCsvFile()
        {
            var periods = new List<PowerPosition>
        {
            new PowerPosition { Period = "1", Volume = 100 },
            new PowerPosition { Period = "2", Volume = 200 }
        };

            string path = "test.csv";
            await _fileService.ExportToCsv(periods, path);
            Assert.True(File.Exists(path), "CSV file should be created.");
            File.Delete(path);
        }

        [Fact]
        public void CreateDirectory_ValidPath_CreatesDirectory()
        {
            string dirPath = "testDirectory";
            _fileService.CreateDirectory(dirPath);
            Assert.True(Directory.Exists(dirPath), "Directory should be created.");
            Directory.Delete(dirPath);
        }

        [Fact]
        public void FileExists_FileExists_ReturnsTrue()
        {
            string path = "existingFile.txt";
            File.WriteAllText(path, "Test content");
            bool result = _fileService.FileExists(path);
            Assert.True(result, "FileExists should return true for existing file.");
            File.Delete(path);
        }

        [Fact]
        public void FileExists_FileDoesNotExist_ReturnsFalse()
        {
            string path = "nonExistingFile.txt";
            bool result = _fileService.FileExists(path);
            Assert.False(result, "FileExists should return false for non-existing file.");
        }
    }

}
