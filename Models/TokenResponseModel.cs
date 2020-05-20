using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.Models
{
    public class TokenResponseModel
    {
        public string Phone { get; set; }
        public string Token { get; set; }

        public TokenResponseModel(string phone, string token)
        {
            Phone = phone;
            Token = token;
        }
    }
}
