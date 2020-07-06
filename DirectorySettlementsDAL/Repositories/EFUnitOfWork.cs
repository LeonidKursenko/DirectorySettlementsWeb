using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Repositories
{
    /// <summary>
    /// EFUnitOfWork class implements IUnitOfWork interface that provides access to the all repositories.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationContext _db;
        private SettlementRepository _settlementRepository;
        private InitialRepository _initialRepository;

        public IRepository<Settlement> Settlements => _settlementRepository;

        public EFUnitOfWork(ApplicationContext context)
        {
            _db = context;
            _settlementRepository = new SettlementRepository(_db);
            _initialRepository = new InitialRepository(_db);
        }

        /// <summary>
        /// Releases the allocated resources for this EfUnitOfWork.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        /// <summary>
        /// Releases the allocated resources for this EfUnitOfWork.
        /// </summary>
        /// <param name="disposing">Disposing determines whether to release resources.</param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _settlementRepository.Dispose();
                }
                _disposed = true;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Fill()
        {
            Settlements.Clear();
            var initialData = _initialRepository.GetAll();
            foreach(var settlement in initialData)
            {
                Settlements.Create(new Settlement
                {
                    Te = settlement.Te,
                    Np = settlement.Np,
                    Nu = settlement.Nu
                });
            }
            Save();
        }
    }
}
