using System.Threading.Tasks;

namespace Factor.IServices
{
    public interface IMessageService
    {
        Task<string> SendSMS(string receptor, long code);
    }
}
