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

        public async Task<bool> VerifyToken(string? token, string expectedAction, double minScore = 0.5)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var secretKey = _configurationService.GetSecretKey();
                if (string.IsNullOrWhiteSpace(secretKey))
                    return false;

                var url =
                    $"https://www.google.com/recaptcha/api/siteverify" +
                    $"?secret={WebUtility.UrlEncode(secretKey)}" +
                    $"&response={WebUtility.UrlEncode(token)}";

                using var client = new HttpClient();
                var httpResult = await client.GetAsync(url);

                if (httpResult.StatusCode != HttpStatusCode.OK)
                    return false;

                var responseString = await httpResult.Content.ReadAsStringAsync();
                var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                if (googleResult == null || !googleResult.Success)
                    return false;

                // v3: validate action to prevent token reuse across endpoints
                if (!string.Equals(googleResult.Action, expectedAction, StringComparison.OrdinalIgnoreCase))
                    return false;

                // v3: validate score
                if (googleResult.Score < minScore)
                    return false;

                // Optional hostname validation
                // if (googleResult.Hostname is null) return false;
                // var allowed = new[] { "irfconnect.com", "www.irfconnect.com" };
                // if (!allowed.Contains(googleResult.Hostname, StringComparer.OrdinalIgnoreCase)) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
