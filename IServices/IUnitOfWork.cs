using Factor.IRepositories;
using Factor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
