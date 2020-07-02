using Factor.IRepositories;
using Factor.Models;

namespace Factor.IServices
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<SMSVerification> VerificationRepository { get; }
        IRepository<PreFactor> PreFactorRepository { get; }
        IRepository<SubmittedFactor> SubmittedFactorRepository { get; }
        IRepository<Contact> ContactRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Image> ImageRepository { get; }
        IRepository<Project> ProjectRepository { get; }

        void Commit();
        void Rollback();
    }
}
