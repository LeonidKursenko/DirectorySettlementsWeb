using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Exceptions;
using DirectorySettlementsDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DirectorySettlementsDAL.Repositories
{
    /// <summary>
    /// SettlementRepository class iImplements IRepository<Settlement> that manages Settlements table.
    /// </summary>
    public class SettlementRepository : IRepository<Settlement>
    {
        /// <value>Gets and sets database context.</value>
        public ApplicationContext Database { get; set; }

        /// <summary>
        /// SettlementRepository constructor.
        /// </summary>
        /// <param name="database">Database context.</param>
        public SettlementRepository(ApplicationContext database)
        {
            Database = database;
        }

        public void Create(Settlement node)
        {
            var settlement = Get(node.Te);
            if(settlement != null)
            {
                throw new CreateOperationException($"Failed to create a new node with existed Te='{node.Te}'.");
            }
            try
            {
                SetParentId(node);
                Database.Settlements.Add(node);
                //Database.SaveChanges();
            }
            catch(CreateOperationException ex)
            {
                throw ex;
            }
            catch(InvalidOperationException ex)
            {
                throw new CreateOperationException("Failed to create a new node. " + ex.Message );
            }
        }

        #region Additional methods
        /// <summary>
        /// Sets ParentId.
        /// </summary>
        /// <param name="settlement">Settlement that needs ParentId.</param>
        public void SetParentId(Settlement settlement)
        {
            Regex regex = new Regex(@"^\d{10}$");
            Match match = regex.Match(settlement.Te);
            if (match.Success == false)
                throw new CreateOperationException($"Failed to create a new node with incorrect format of the Te='{settlement.Te}'.");
            long te = Int64.Parse(settlement.Te);
            long parentTe = GetParentId(te);

            string parentId = parentTe != 0 ? parentTe.ToString("D10") : null;
            if(parentId != null && Get(parentId) == null)
                throw new CreateOperationException($"Failed to create a new node with incorrect Te='{settlement.Te}' " +
                    $"because it has no parent element with Te='{parentId}'.");
            settlement.ParentId = parentId;
        }

        /// <summary>
        /// Generates ParentId.
        /// </summary>
        /// <param name="te">Te code of a child element.</param>
        /// <returns>ParentId.</returns>
        private static long GetParentId(long te)
        {
            long parentTe = 0;
            // Fourth category.
            if (te % 100 != 0) return te / 100 * 100;
            // Third category.
            //if (te % 1_000 != 0) return te / 1_000 * 1_000;
            if (te % 10_000 != 0) return te / 10_000 * 10_000;
            if (te % 100_000 != 0) return te / 100_000 * 100_000;
            // Second category.
            //if (te % 1_000_000 != 0) return te / 1_000_000 * 1_000_000;
            //if (te % 10_000_000 != 0) return te / 10_000_000 * 10_000_000;
            if (te % 100_000_000 != 0) return te / 100_000_000 * 100_000_000;
            return parentTe;
        }
        #endregion

        public void Delete(string te)
        {
            Settlement settlement = Get(te);
            if (settlement == null)
                throw new DeleteOperationException($"Failed to delete node with unknown Te='{te}'.");
            if (settlement.Children.Count() > 0)
                throw new DeleteOperationException($"Failed to delete node Te='{te}' with child nodes.");
            try
            {
                Database.Settlements.Remove(settlement);
                //Database.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                throw new DeleteOperationException($"Failed to delete node Te='{te}'. " + ex.Message);
            }
        }

        public void DeleteAll(string te)
        {
            Settlement settlement = Get(te);
            if (settlement == null)
                throw new DeleteOperationException($"Failed to delete node with unknown Te='{te}'.");
            while(settlement.Children.Count() > 0)
            {
                DeleteAll(settlement.Children.First().Te);
            }
            Delete(te);
            Database.SaveChanges();
        }

        public void Clear()
        {
            var parents = GetAll();
            while(parents.Count() > 0)
            {
                DeleteAll(parents.First().Te);
            }
        }

        public IEnumerable<Settlement> Find(Func<Settlement, bool> predicate)
        {
            return Database.Settlements.Where(predicate).ToList();
        }

        public Settlement Get(string te)
        {
            return Database.Settlements.Find(te);
        }

        public IEnumerable<Settlement> GetAll()
        {
            return Database.Settlements;
        }

        public void Update(Settlement node)
        {
            Database.Entry(node).State = EntityState.Modified;
        }

        /// <summary>
        /// Releases the allocated resources for this SettlementRepository.
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
