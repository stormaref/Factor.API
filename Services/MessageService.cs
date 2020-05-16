﻿using Factor.IServices;
using Factor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private SMSTokenRequest SMSToken;

        public MessageService(ILogger<MessageService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            SMSToken = new SMSTokenRequest(_configuration.GetSection("SMS").GetSection("UserApiKey").Value, _configuration.GetSection("SMS").GetSection("SecretKey").Value);
        }

        public async Task<string> SendSMS(string receptor, long code)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = new StringContent(JsonSerializer.Serialize(SMSToken), Encoding.UTF8, "application/json");
                string url = "http://RestfulSms.com/api/Token";
                var response = await client.PostAsync(url, data);
                var @object = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (@object.Value<bool>("IsSuccessful"))
                {
                    string token = @object.Value<string>("TokenKey");
                    client.DefaultRequestHeaders.Add("x-sms-ir-secure-token", token);
                    var messageData = new StringContent(JsonSerializer.Serialize(new SMSVerificationRequest(code, receptor)), Encoding.UTF8, "application/json");
                    var messageResponse = await client.PostAsync("http://RestfulSms.com/api/VerificationCode", messageData);
                    JObject messageJObject = JObject.Parse(await messageResponse.Content.ReadAsStringAsync());
                    if (messageJObject.Value<bool>("IsSuccessful"))
                    {
                        return messageJObject.Value<string>("Message");
                    }
                    else
                    {
                        throw new Exception("Phone is incorrent or sms service error");
                    }
                }
                else
                {
                    throw new Exception("Security codes are incorrect contact back-end");
                }
            }
        }
    }
}
