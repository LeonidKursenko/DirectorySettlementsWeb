using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Repositories
{
    internal class InitialRepository
    {
        ApplicationContext Database { get; set; }

        /// <summary>
        /// SettlementRepository constructor.
        /// </summary>
        /// <param name="database">Database context.</param>
        public InitialRepository(ApplicationContext database)
        {
            Database = database;
        }
        public IEnumerable<InitialTable> GetAll()
        {
            return Database.InitialTable;
        }

        /// <summary>
        /// Releases the allocated resources for this repository.
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
