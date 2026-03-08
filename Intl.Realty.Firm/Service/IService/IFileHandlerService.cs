namespace Intl.Realty.Firm.Service.IService
{
    public interface IFileHandlerService
    {
        Task<(string, string, string)> UploadFile(IFormFile file, string directory);
        Task<(byte[], string, string)> DownloadFile(string directory, string fileName, string fileExtension);
        Task<bool> DeleteFile(string fullPath);
        Task<bool> CheckIfFileExists(string fullPath);
    }
}
