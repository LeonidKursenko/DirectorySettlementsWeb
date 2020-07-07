using DirectorySettlementsDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        Task SaveAsync();

        /// <summary>
        /// Fills Settlements table by using data from InitialTable.
        /// </summary>
        Task FillAsync();

        /// <summary>
        /// Removes All data from Settlements table.
        /// </summary>
        Task ClearAsync();
    }
}
