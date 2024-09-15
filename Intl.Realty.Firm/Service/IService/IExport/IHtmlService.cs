namespace Intl.Realty.Firm.Service.IService.IExport
{
    public interface IHtmlService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
