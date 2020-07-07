using AutoMapper;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DirectorySettlementsDAL.Repositories
{
    /// <summary>
    /// EFUnitOfWork class implements IUnitOfWork interface that provides access to the all repositories.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;
        private readonly SettlementRepository _settlementRepository;
        private readonly InitialRepository _initialRepository;
        private readonly Mapper _mapper;

        public IRepository<Settlement> Settlements => _settlementRepository;

        public EFUnitOfWork(ApplicationContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<InitialTable, Settlement>());
            _mapper = new Mapper(config);
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
                    _initialRepository.Dispose();
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
            var initialData = _initialRepository.GetAll().ToList();
            
            var settlements = _mapper.Map<List<Settlement>>(initialData);
            Settlements.AddRange(settlements);
        }

        public void Clear()
        {
            Settlements.Clear();
        }
    }
}
