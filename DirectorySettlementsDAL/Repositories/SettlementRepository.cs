using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Exceptions;
using DirectorySettlementsDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<string> _allExistsTe;

        public void Create(Settlement node)
        {
            _allExistsTe = Database.Settlements.Select(s => s.Te).ToList();
            Add(node);
            Database.SaveChanges();
        }

        #region Additional methods
        public void AddRange(IEnumerable<Settlement> nodes)
        {
            _allExistsTe = Database.Settlements.Select(s => s.Te).ToList();
            var settlements = nodes.ToList();
            foreach (var settlement in settlements)
            {
                SetParentId(settlement);
                _allExistsTe.Add(settlement.Te);
            }
            try
            {
                Database.Settlements.AddRange(settlements);
            }
            catch(Exception ex)
            {
                throw new CreateOperationException($"Failed to add range of settlements. " + ex.Message);
            }
            Database.SaveChanges();
        }

        /// <summary>
        /// Adds one node to the Settlements repository without savings.
        /// </summary>
        /// <param name="node"></param>
        private void Add(Settlement node)
        {
            
            var settlement = Get(node.Te);
            if (settlement != null)
            {
                throw new CreateOperationException($"Failed to create a new node with existed Te='{node.Te}'.");
            }
            try
            {
                SetParentId(node);
                Database.Settlements.Add(node);
                _allExistsTe.Add(node.Te);
            }
            catch (CreateOperationException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new CreateOperationException($"Failed to create a new node with Te={node.Te}. " + ex.Message);
            }
        }

        /// <summary>
        /// Sets ParentId.
        /// </summary>
        /// <param name="settlement">Settlement that needs ParentId.</param>
        private void SetParentId(Settlement settlement)
        {
            Regex regex = new Regex(@"^\d{10}$");
            Match match = regex.Match(settlement.Te);
            if (match.Success == false)
                throw new CreateOperationException($"Failed to create a new node with incorrect format of the Te='{settlement.Te}'.");
            long te = Int64.Parse(settlement.Te);
            string parentTe = GetParentId(te);
            bool isValidParent = ValidateParentTe(settlement.Te, parentTe);
            if (isValidParent == false)
                throw new CreateOperationException($"Failed to create a new node with incorrect Te='{settlement.Te}' " +
                    $"because it has no parent element with Te='{parentTe}'.");
            settlement.ParentId = parentTe;
        }

        /// <summary>
        /// Generates ParentId.
        /// </summary>
        /// <param name="te">Te code of a child element.</param>
        /// <returns>ParentId.</returns>
        private string GetParentId(long te)
        {
            for (int i = 100; i <= 100_000_000; i *= 10)
            {
                if (te % i != 0)
                {
                    long parentTe = te / i * i;
                    string parentId = parentTe.ToString("D10");
                    if (_allExistsTe.Contains(parentId) == true) return parentId;
                    //Settlement parent = Get(parentId);
                    //if (parent != null) return parentId;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets category of settlement.
        /// Fourth category 1-2 digits number.Example, 01 222 333 44
        /// Third category 3-5 digits number.Example, 01 222 333 00
        /// Second category 6-8 digits number.Example, 01 222 000 00
        /// First category 9-10 digits number. Example, 01 000 000 00
        /// </summary>
        /// <param name="te">Te code of the settlement.</param>
        /// <returns></returns>
        private int GetCategory(string te)
        {
            long id = Int64.Parse(te);
            for (int i = 100, category = 4; i <= 100_000_000; i *= 1000, category--)
                if (id % i != 0) return category;

            // First category.
            return 1;
        }

        /// <summary>
        /// Checks if settlement has valid parent.
        /// </summary>
        /// <param name="te">Settlement Te.</param>
        /// <param name="parentTe">Parent Te.</param>
        /// <returns>Returns false if difference between categories of the settlement and parent settlement
        /// is bigger then one.</returns>
        private bool ValidateParentTe(string te, string parentTe)
        {
            int category = GetCategory(te);
            if (category == 1 && parentTe == null) return true;
            if (parentTe == null) return false;
            if (category - GetCategory(parentTe) > 1) return false;
            return true;
        }
        #endregion

        public void Delete(string te)
        {
            Settlement settlement = Get(te);
            if (settlement == null)
                throw new DeleteOperationException($"Failed to delete node with unknown Te='{te}'.");
            if (settlement.Children.Any() == true)
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
            DeleteAll(settlement);
            Delete(settlement.Te);
            //Database.Settlements.Remove(settlement);
            Database.SaveChanges();
        }

        private void DeleteAll(Settlement settlement)
        {
            var childrens = settlement.Children.ToList();
            foreach (var child in childrens)
            {
                DeleteAll(child);
                Delete(child.Te);
                //Database.Settlements.Remove(child);
                //Database.SaveChanges();
            }
            Database.SaveChanges();
        }
        public void Clear()
        {
            Database.Database.ExecuteSqlCommand("TRUNCATE TABLE [Settlements]");
            //Database.Settlements.RemoveRange(Database.Settlements);
            //Database.SaveChanges();
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
