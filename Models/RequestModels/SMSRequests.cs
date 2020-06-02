using Newtonsoft.Json;

namespace Factor.Models.RequestModels
{
    public class SMSTokenRequestModel
    {
        [JsonProperty("UserApiKey")]
        public string UserApiKey { get; set; }

        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }

        public SMSTokenRequestModel(string userApiKey, string secretKey)
        {
            UserApiKey = userApiKey;
            SecretKey = secretKey;
        }
    }
}
