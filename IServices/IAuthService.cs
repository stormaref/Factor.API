using Factor.Models;
using System.Threading.Tasks;

namespace Factor.IServices
{
    public interface IAuthService
    {
        string CreateToken(User user);
        Task<User> GetUser(string id);
    }
}
