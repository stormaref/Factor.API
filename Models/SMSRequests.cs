using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class SMSTokenRequest
    {
        [JsonProperty("UserApiKey")]
        public string UserApiKey { get; set; }

        [JsonProperty("SecretKey")]
        public string SecretKey { get; set; }

        public SMSTokenRequest(string userApiKey, string secretKey)
        {
            UserApiKey = userApiKey;
            SecretKey = secretKey;
        }
    }

    public class SMSVerificationRequest
    {
        [JsonProperty("Code")]
        public long Code { get; set; }

        [JsonProperty("MobileNumber")]
        public string MobileNumber { get; set; }

        public SMSVerificationRequest(long code, string mobileNumber)
        {
            Code = code;
            MobileNumber = mobileNumber;
        }
    }
}
