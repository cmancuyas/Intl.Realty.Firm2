using Intl.Realty.Firm.Service.IService.IConfiguration;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IConfigurationService: IConfigurationEmail,IConfigurationNotification, IConfigurationGoogleCaptcha
    {
        string GetDefaultUploadPathFromConfig();
        string[] GetSupportEmail();
        string[] GetSupportEmailCC();
    }
}
