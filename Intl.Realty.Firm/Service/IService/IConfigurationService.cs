using Intl.Realty.Firm.Service.IService.IConfiguration;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IConfigurationService: IConfigurationEmail
    {
        string GetDefaultUploadPathFromConfig();
    }
}
