using Factor.IServices;
using Factor.Models.RequestModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Factor.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IConfiguration _configuration;
        private readonly SMSTokenRequestModel SMSToken;
        private readonly string AppHash;

        public MessageService(ILogger<MessageService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            SMSToken = new SMSTokenRequestModel(_configuration.GetSection("SMS").GetSection("UserApiKey").Value, _configuration.GetSection("SMS").GetSection("SecretKey").Value);
            AppHash = _configuration.GetValue<string>("AppHash");
        }

        public async Task<string> SendSMS(string receptor, long code)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent data = new StringContent(JsonSerializer.Serialize(SMSToken), Encoding.UTF8, "application/json");
                string url = "http://RestfulSms.com/api/Token";
                HttpResponseMessage response = await client.PostAsync(url, data);
                JObject @object = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (@object.Value<bool>("IsSuccessful"))
                {
                    string token = @object.Value<string>("TokenKey");
                    client.DefaultRequestHeaders.Add("x-sms-ir-secure-token", token);
                    string message = string.Format("<#> {0}{1}{2}{3}{4}", _configuration.GetValue<string>("MessageText"), Environment.NewLine, code, Environment.NewLine, AppHash);
                    SMSVerificationRequestModel model = new SMSVerificationRequestModel(message, receptor, _configuration.GetValue<string>("LineNumber"));
                    StringContent messageData = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                    HttpResponseMessage messageResponse = await client.PostAsync("http://RestfulSms.com/api/MessageSend", messageData);
                    JObject messageJObject = JObject.Parse(await messageResponse.Content.ReadAsStringAsync());
                    if (messageJObject.Value<bool>("IsSuccessful"))
                    {
                        return messageJObject.Value<string>("Message");
                    }
                    else
                    {
                        Exception ex = new Exception("Phone is incorrent or sms service error");
                        _logger.Log(LogLevel.Warning, ex, ex.Message, @object);
                        throw ex;
                    }
                }
                else
                {
                    Exception ex = new Exception("Security codes are incorrect contact back-end");
                    _logger.Log(LogLevel.Warning, ex, ex.Message, @object);
                    throw ex;
                }
            }
        }
    }
}
