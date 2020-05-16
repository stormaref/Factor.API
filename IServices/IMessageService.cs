using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.IServices
{
    public interface IMessageService
    {
        Task<string> SendSMS(string receptor, long code);
    }
}
