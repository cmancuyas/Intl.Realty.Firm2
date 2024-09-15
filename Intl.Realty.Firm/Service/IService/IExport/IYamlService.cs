namespace Intl.Realty.Firm.Service.IService.IExport
{
    public interface IYamlService
    {
        byte[] Write<T>(IList<T> registers);
    }
}
