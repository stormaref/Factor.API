using Factor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.IServices
{
    interface IAuthService
    {
        string CreateToken(User user);
    }
}
