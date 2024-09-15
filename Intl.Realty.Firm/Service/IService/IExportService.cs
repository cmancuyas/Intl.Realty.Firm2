namespace Intl.Realty.Firm.Service.IService
{
    public interface IExportService<TModel> where TModel : class
    {
        Task<byte[]> ExportToExcel(List<TModel> modelList);

        byte[] ExportToCsv(List<TModel> modelList);

        byte[] ExportToHtml(List<TModel> modelList);

        byte[] ExportToJson(List<TModel> modelList);

        byte[] ExportToXml(List<TModel> modelList);

        byte[] ExportToYaml(List<TModel> modelList);
    }
}
