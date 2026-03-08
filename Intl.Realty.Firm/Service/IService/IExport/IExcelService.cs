namespace Intl.Realty.Firm.Service.IService.IExport
{
    public interface IExcelService
    {
        Task<byte[]> Write<T>(IList<T> modelList);
    }
}
