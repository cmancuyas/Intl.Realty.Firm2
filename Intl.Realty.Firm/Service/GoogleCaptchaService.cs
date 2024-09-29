using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Service.IService;
using Newtonsoft.Json;
using System.Net;

namespace Intl.Realty.Firm.Service
{
    public class GoogleCaptchaService : IGoogleCaptchaService
    {
        private readonly IConfigurationService _configurationService;

        public GoogleCaptchaService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var secretKey = _configurationService.GetSecretKey();
                var siteKey = _configurationService.GetSiteKey();
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

                using(var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }

                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    return googleResult.Success && googleResult.Score >= 0.5;
                }
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
