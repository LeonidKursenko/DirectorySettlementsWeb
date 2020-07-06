using DirectorySettlementsDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Interfaces
{
    /// <summary>
    /// IUnitOfWork interface provides access to the all repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <value>Gets access to the Settlements Repository.</value>
        IRepository<Settlement> Settlements { get; }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Fills Settlements table by using data from InitialTable.
        /// </summary>
        void Fill();
    }
}
