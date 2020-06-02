using Newtonsoft.Json;

namespace Factor.Models.RequestModels
{
    public class SMSVerificationRequestModel
    {
        [JsonProperty("Code")]
        public long Code { get; set; }

        [JsonProperty("MobileNumber")]
        public string MobileNumber { get; set; }

        public SMSVerificationRequestModel(long code, string mobileNumber)
        {
            Code = code;
            MobileNumber = mobileNumber;
        }
    }
}
