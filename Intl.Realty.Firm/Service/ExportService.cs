using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Service.IService.IExport;

namespace Intl.Realty.Firm.Service
{
    public class ExportService<TModel> : IExportService<TModel> where TModel : class
    {
        private readonly IExcelService _excelService;
        private readonly ICsvService _csvService;
        private readonly IHtmlService _htmlService;
        private readonly IJsonService _jsonService;
        private readonly IXmlService _xmlService;
        private readonly IYamlService _yamlService;

        public ExportService(IExcelService excelService, ICsvService csvService, IHtmlService htmlService, IJsonService jsonService, IXmlService xmlService, IYamlService yamlService)
        {
            _excelService = excelService;
            _csvService = csvService;
            _htmlService = htmlService;
            _jsonService = jsonService;
            _xmlService = xmlService;
            _yamlService = yamlService;
        }

        public async Task<byte[]> ExportToExcel(List<TModel> modelList)
        {
            return await _excelService.Write(modelList);
        }

        public byte[] ExportToCsv(List<TModel> modelList)
        {
            return _csvService.Write(modelList);
        }

        public byte[] ExportToHtml(List<TModel> modelList)
        {
            return _htmlService.Write(modelList);
        }

        public byte[] ExportToJson(List<TModel> modelList)
        {
            return _jsonService.Write(modelList);
        }

        public byte[] ExportToXml(List<TModel> modelList)
        {
            return _xmlService.Write(modelList);
        }

        public byte[] ExportToYaml(List<TModel> modelList)
        {
            return _yamlService.Write(modelList);
        }
    }
}
