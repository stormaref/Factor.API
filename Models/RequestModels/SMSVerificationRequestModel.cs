using Newtonsoft.Json;
using System.Collections.Generic;

namespace Factor.Models.RequestModels
{
    public class SMSVerificationRequestModel
    {
        [JsonProperty("MobileNumbers")]
        public List<string> MobileNumbers { get; set; }

        [JsonProperty("Messages")]
        public List<string> Messages { get; set; }

        [JsonProperty("LineNumber")]
        public string LineNumber { get; set; }

        public SMSVerificationRequestModel(string message, string mobileNumber, string lineNumber)
        {
            Messages = new List<string> { message };
            MobileNumbers = new List<string> { mobileNumber };
            LineNumber = lineNumber;
        }
    }
}
