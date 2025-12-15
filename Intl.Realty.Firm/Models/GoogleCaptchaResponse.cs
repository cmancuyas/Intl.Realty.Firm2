using Newtonsoft.Json;

namespace Intl.Realty.Firm.Models
{
    public class GoogleCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("action")]
        public string? Action { get; set; }

        [JsonProperty("hostname")]
        public string? Hostname { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }

        [JsonProperty("error-codes")]
        public string[]? ErrorCodes { get; set; }
    }
}
