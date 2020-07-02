using Factor.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Factor.IServices
{
    public interface IAuthService
    {
        string CreateToken(User user);
        Task<User> GetUser(string id);
        Task<User> GetUser(HttpContext context);
    }
}
