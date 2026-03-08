using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utilities;
using Microsoft.AspNetCore.StaticFiles;

namespace Intl.Realty.Firm.Service
{
    public class FileHandlerService : IFileHandlerService
    {
        public Task<bool> CheckIfFileExists(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
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
                throw ex;
            }
        }

        public async Task<(string, string, string)> UploadFile(IFormFile iFormFile, string directory)
        {
            string fileNameWithoutExtension = string.Empty;
            string fileExtension = string.Empty;
            string originalFileName = string.Empty;

            try
            {
                if (iFormFile != null && iFormFile?.Length > 0)
                {
                    (fileNameWithoutExtension, fileExtension, originalFileName) = await UploadFileToDirectory(iFormFile, directory);
                }
                return (fileNameWithoutExtension, fileExtension, originalFileName);
            }
            catch (Exception ex)
            {
                throw new Exception("File could not be uploaded", ex);
            }
        }
        private async Task<(string, string, string)> UploadFileToDirectory(IFormFile iFormFile, string directory)
        {
            string fileNameWithoutExtension = string.Empty;
            string fileExtension = string.Empty;
            string originalFileName = string.Empty;

            FileInfo fileInfo = new FileInfo(iFormFile.FileName);

            fileExtension = fileInfo.Extension;

            originalFileName = iFormFile.FileName;

            var originalFileNameWithoutExtension = FileHandler.GetFileNameWithoutExtension(originalFileName);

            var fullFileName = originalFileNameWithoutExtension + "_" + DateTime.Now.Ticks.ToString() + fileExtension;

            fileNameWithoutExtension = FileHandler.GetFileNameWithoutExtension(fullFileName);

            var filePath = FileHandler.GetFilePath(directory, fullFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await iFormFile.CopyToAsync(fileStream);
            }
            return (fileNameWithoutExtension, fileExtension, originalFileName);
        }
    }
}
