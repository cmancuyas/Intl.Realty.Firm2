namespace Intl.Realty.Firm.Service.IService.IExport
{
    public interface IXmlService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
