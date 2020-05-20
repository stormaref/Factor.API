using Factor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Factor.IServices
{
    public interface IAuthService
    {
        string CreateToken(User user);
    }
}
