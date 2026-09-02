using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utilities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace Intl.Realty.Firm.Service
{
    public class FileHandlerService : IFileHandlerService
    {
        private readonly ILogger<FileHandlerService> _logger;

        public FileHandlerService(ILogger<FileHandlerService> logger)
        {
            _logger = logger;
        }

        public Task<bool> CheckIfFileExists(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                return Task.FromResult(false);

            return Task.FromResult(File.Exists(fullPath) || File.Exists(Path.Combine(Directory.GetCurrentDirectory(), fullPath)));
        }

        public Task<bool> DeleteFile(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                return Task.FromResult(false);

            var candidates = new[]
            {
                fullPath,
                Path.IsPathRooted(fullPath) ? fullPath : Path.Combine(Directory.GetCurrentDirectory(), fullPath)
            }.Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var path in candidates)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public async Task<(byte[], string, string)> DownloadFile(string directory, string fileName, string fileExtension)
        {
            try
            {
                var filePath = FileHandler.GetFilePath(directory, fileName + fileExtension);

                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var readAllBytesAsync = await File.ReadAllBytesAsync(filePath);

                return (readAllBytesAsync, contentType, Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Download failed for directory {Directory}, file {FileName}{Extension}", directory, fileName, fileExtension);
                throw;
            }
        }

        public async Task<(string, string, string)> UploadFile(IFormFile iFormFile, string directory)
        {
            string fileNameWithoutExtension = string.Empty;
            string fileExtension = string.Empty;
            string originalFileName = string.Empty;

            _logger.LogInformation("Upload request received for directory {Directory}", directory);

            try
            {
                if (iFormFile == null || iFormFile.Length <= 0)
                {
                    _logger.LogWarning("No files submitted");
                    return (fileNameWithoutExtension, fileExtension, originalFileName);
                }

                (fileNameWithoutExtension, fileExtension, originalFileName) = await UploadFileToDirectory(iFormFile, directory);
                _logger.LogInformation("File uploaded successfully: {OriginalFileName} -> {StoredFileName}{Extension}",
                    originalFileName, fileNameWithoutExtension, fileExtension);
                return (fileNameWithoutExtension, fileExtension, originalFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed for file {FileName} into directory {Directory}", iFormFile?.FileName, directory);
                throw new Exception("File could not be uploaded", ex);
            }
        }

        private async Task<(string, string, string)> UploadFileToDirectory(IFormFile iFormFile, string directory)
        {
            FileInfo fileInfo = new FileInfo(iFormFile.FileName);

            var fileExtension = fileInfo.Extension;
            var originalFileName = iFormFile.FileName;
            var originalFileNameWithoutExtension = FileHandler.GetFileNameWithoutExtension(originalFileName);
            var safeOriginalName = TransactionHelper.SanitizePathSegment(originalFileNameWithoutExtension, "file");

            var fullFileName = safeOriginalName + "_" + DateTime.Now.Ticks.ToString() + fileExtension;
            var fileNameWithoutExtension = FileHandler.GetFileNameWithoutExtension(fullFileName);

            var filePath = FileHandler.GetFilePath(directory, fullFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await iFormFile.CopyToAsync(fileStream);
            }

            return (fileNameWithoutExtension, fileExtension, originalFileName);
        }
    }
}
