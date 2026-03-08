namespace Intl.Realty.Firm.Service.IService.IExport
{
    public interface IJsonService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
