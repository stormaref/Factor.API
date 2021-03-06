﻿using Factor.IRepositories;
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

        private IRepository<PreFactor> _preFactorRepository;
        public IRepository<PreFactor> PreFactorRepository => _preFactorRepository ??= new Repository<PreFactor>(_databasecontext);

        private IRepository<SubmittedFactor> _submittedFactorRepository;
        public IRepository<SubmittedFactor> SubmittedFactorRepository => _submittedFactorRepository ??= new Repository<SubmittedFactor>(_databasecontext);

        private IRepository<Contact> _contactFactorRepository;
        public IRepository<Contact> ContactRepository => _contactFactorRepository ??= new Repository<Contact>(_databasecontext);

        private IRepository<Product> _productRepository;
        public IRepository<Product> ProductRepository => _productRepository ??= new Repository<Product>(_databasecontext);

        private IRepository<Image> _imageRepository;
        public IRepository<Image> ImageRepository => _imageRepository ??= new Repository<Image>(_databasecontext);

        private IRepository<Project> _projectRepository;
        public IRepository<Project> ProjectRepository => _projectRepository ??= new Repository<Project>(_databasecontext);

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
