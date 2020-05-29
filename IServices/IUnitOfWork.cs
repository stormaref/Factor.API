using Factor.IRepositories;
using Factor.Models;

namespace Factor.IServices
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<SMSVerification> VerificationRepository { get; }
        IRepository<Models.Factor> FactorRepository { get; }
        void Commit();
        void Rollback();
    }
}
