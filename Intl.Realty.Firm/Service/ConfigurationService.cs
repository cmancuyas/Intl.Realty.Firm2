using Intl.Realty.Firm.Service.IService;

namespace Intl.Realty.Firm.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetDefaultUploadPathFromConfig()
        {
            var defaultPath = _configuration["FileUpload:DefaultPath"];
            if (defaultPath != null)
            {
                return defaultPath;
            }
            return "";
        }

        public string GetDisplayName()
        {
            var displayName = _configuration["EmailSettings:DisplayName"];
            if (displayName != null)
            {
                return displayName;
            }
            return "";
        }

        public string GetEmail()
        {
            var email = _configuration["EmailSettings:Email"];
            if (email != null)
            {
                return email;
            }
            return "";
        }

        public string GetHost()
        {
            var host = _configuration["EmailSettings:Host"];
            if (host != null)
            {
                return host;
            }
            return "";
        }

        public string GetNotificationProvider()
        {
            var item = _configuration["NotificationProvider"];
            if (item != null)
            {
                return item;
            }
            return "";
        }

        public string GetPassword()
        {
            var password = _configuration["EmailSettings:Password"];
            if (password != null)
            {
                return password;
            }
            return "";
        }

        public string GetPort()
        {
            var port = _configuration["EmailSettings:Port"];
            if (port != null)
            {
                return port;
            }
            return "";
        }

        public string GetSecretKey()
        {
            var secretKey = _configuration["GoogleReCaptcha:SecretKey"];
            if (secretKey != null)
            {
                return secretKey;
            }
            return "";
        }

        public string GetSiteKey()
        {
            var siteKey = _configuration["GoogleReCaptcha:SiteKey"];
            if (siteKey != null)
            {
                return siteKey;
            }
            return "";
        }
        public string GetSupportEmail()
        {
            var supportEmail = _configuration["SupportEmail"];
            if (supportEmail != null)
            {
                return supportEmail;
            }
            return "";
        }

    }
}
