using Factor.IRepositories;
using Factor.IServices;
using Factor.Models;
using Factor.Repositories;

namespace Factor.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databasecontext;
        public UnitOfWork(DatabaseContext databasecontext)
        {
            _databasecontext = databasecontext;
        }

        private IRepository<User> _userRepository;
        public IRepository<User> UserRepository => _userRepository ??= new Repository<User>(_databasecontext);

        private IRepository<SMSVerification> _verificationRepository;
        public IRepository<SMSVerification> VerificationRepository => _verificationRepository ??= new Repository<SMSVerification>(_databasecontext);

        private IRepository<Models.FactorItem> _factorRepository;
        public IRepository<Models.FactorItem> FactorRepository => _factorRepository ??= new Repository<Models.FactorItem>(_databasecontext);

        public void Commit()
        {
            _databasecontext.SaveChanges();
        }

        public void Rollback()
        {
            _databasecontext.Dispose();
        }
    }
}
