using Evva.Context.Repository;
using Evva.Context;
using Evva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evva.Context.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private DietManagerContext db = new DietManagerContext();
        private DietManagerDBRepository<User> userRepository;
        private DietManagerDBRepository<Report> reportRepository;
        private DietManagerDBRepository<Product> productRepository;
        private DietManagerDBRepository<FoodCategory> foodCategoryRepository;
        private DietManagerDBRepository<UsersDatum> userDatumRepository;
        private DietManagerDBRepository<UsersParam> userParamRepository;

        public DietManagerDBRepository<UsersParam> UserParamRepository
        {
            get
            {
                if (userParamRepository == null)
                    userParamRepository = new DietManagerDBRepository<UsersParam>(db);
                return userParamRepository;
            }
        }

        public DietManagerDBRepository<UsersDatum> UserDatumRepository
        {
            get
            {
                if (userDatumRepository == null)
                    userDatumRepository = new DietManagerDBRepository<UsersDatum>(db);
                return userDatumRepository;
            }
        }

        public DietManagerDBRepository<FoodCategory> FoodCategoryRepository
        {
            get
            {
                if (foodCategoryRepository == null)
                    foodCategoryRepository = new DietManagerDBRepository<FoodCategory>(db);
                return foodCategoryRepository;
            }
        }

        public DietManagerDBRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                    productRepository = new DietManagerDBRepository<Product>(db);
                return productRepository;
            }
        }

        public DietManagerDBRepository<Report> ReportRepository
        {
            get
            {
                if (reportRepository == null)
                    reportRepository = new DietManagerDBRepository<Report>(db);
                return reportRepository;
            }
        }

        public DietManagerDBRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new DietManagerDBRepository<User>(db);
                return userRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

