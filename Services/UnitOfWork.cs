using Factor.IRepositories;
using Factor.IServices;
using Factor.Models;
using Factor.Repositories;

namespace Factor.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databasecontext;

        private IRepository<User> _userRepository;
        public IRepository<User> UserRepository
        {
            get { return _userRepository ??= new Repository<User>(_databasecontext); }
        }

        private IRepository<SMSVerification> _verificationRepository;
        public IRepository<SMSVerification> VerificationRepository
        {
            get { return _verificationRepository ??= new Repository<SMSVerification>(_databasecontext); }
        }

        private IRepository<Models.Factor> _factorRepository;
        public IRepository<Models.Factor> FactorRepository
        {
            get { return _factorRepository ??= new Repository<Models.Factor>(_databasecontext); }
        }

        public UnitOfWork(DatabaseContext databasecontext)
        {
            _databasecontext = databasecontext;
        }

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
