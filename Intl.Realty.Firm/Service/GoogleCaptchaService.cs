using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Service.IService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Intl.Realty.Firm.Service
{
    public class GoogleCaptchaService : IGoogleCaptchaService
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<GoogleCaptchaService> _logger;

        public GoogleCaptchaService(
            IConfigurationService configurationService,
            ILogger<GoogleCaptchaService> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        public async Task<bool> VerifyToken(string? token, string expectedAction, double minScore = 0.5)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("reCAPTCHA token was missing for action {Action}.", expectedAction);
                return false;
            }

            try
            {
                var secretKey = _configurationService.GetSecretKey();
                if (string.IsNullOrWhiteSpace(secretKey))
                {
                    _logger.LogError("reCAPTCHA secret key is not configured.");
                    return false;
                }

                var url =
                    $"https://www.google.com/recaptcha/api/siteverify" +
                    $"?secret={WebUtility.UrlEncode(secretKey)}" +
                    $"&response={WebUtility.UrlEncode(token)}";

                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
                var httpResult = await client.GetAsync(url);

                if (httpResult.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("reCAPTCHA siteverify returned HTTP {StatusCode}.", httpResult.StatusCode);
                    return false;
                }

                var responseString = await httpResult.Content.ReadAsStringAsync();
                var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                if (googleResult == null || !googleResult.Success)
                {
                    _logger.LogWarning(
                        "reCAPTCHA verification failed. Errors: {Errors}",
                        googleResult?.ErrorCodes == null ? "(none)" : string.Join(",", googleResult.ErrorCodes));
                    return false;
                }

                if (!string.Equals(googleResult.Action, expectedAction, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "reCAPTCHA action mismatch. Expected {Expected}, got {Actual}.",
                        expectedAction, googleResult.Action);
                    return false;
                }

                if (googleResult.Score < minScore)
                {
                    _logger.LogWarning(
                        "reCAPTCHA score too low for action {Action}. Score={Score}, Min={MinScore}.",
                        expectedAction, googleResult.Score, minScore);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "reCAPTCHA verification threw for action {Action}.", expectedAction);
                return false;
            }
        }
    }
}
