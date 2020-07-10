using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Exceptions;
using DirectorySettlementsDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DirectorySettlementsDAL.Repositories
{
    /// <summary>
    /// SettlementRepository class implements IRepository that manages Settlements table.
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

        public async Task CreateAsync(Settlement node)
        {
            _allExistsTe = Database.Settlements.Select(s => s.Te).ToList();
            await AddAsync(node);
            await Database.SaveChangesAsync();
        }

        #region Additional methods
        public async Task AddRangeAsync(IEnumerable<Settlement> nodes)
        {
            _allExistsTe = await Database.Settlements.Select(s => s.Te).ToListAsync();
            var settlements = nodes.ToList();
            foreach (var settlement in settlements)
            {
                SetParentId(settlement);
                _allExistsTe.Add(settlement.Te);
            }
            try
            {
                await Database.Settlements.AddRangeAsync(settlements);
            }
            catch(Exception ex)
            {
                throw new CreateOperationException($"Failed to add range of settlements. " + ex.Message);
            }
            await Database.SaveChangesAsync();
        }

        /// <summary>
        /// Adds one node to the Settlements repository without savings.
        /// </summary>
        /// <param name="node"></param>
        private async Task AddAsync(Settlement node)
        {
            
            var settlement = await GetAsync(node.Te);
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

        public async Task DeleteAsync(string te)
        {
            Settlement settlement = await GetAsync(te);
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

        public async Task DeleteAllAsync(string te)
        {
            Settlement settlement = await GetAsync(te);
            if (settlement == null)
                throw new DeleteOperationException($"Failed to delete node with unknown Te='{te}'.");
            await DeleteAllAsync(settlement);
            await DeleteAsync(settlement.Te);
            await SaveAsync(te);
        }
        private async Task SaveAsync(string te)
        {
            try
            {
                await Database.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new DeleteOperationException($"Failed to delete node Te='{te}'. " + ex.Message);
            }
        }

        private async Task DeleteAllAsync(Settlement settlement)
        {
            var childrens = settlement.Children.ToList();
            foreach (var child in childrens)
            {
                await DeleteAllAsync(child);
                await DeleteAsync(child.Te);
            }
            await SaveAsync(settlement.Te);
        }
        public async Task ClearAsync()
        {
            try
            {
                _ = await Database.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Settlements]");
            }
            catch
            {
                Database.Settlements.RemoveRange(Database.Settlements);
                await Database.SaveChangesAsync();
            }
        }

        public IEnumerable<Settlement> Find(Func<Settlement, bool> predicate)
        {
            return Database.Settlements.Include(s => s.Children).Where(predicate);
        }

        public async Task<Settlement> GetAsync(string te)
        {
            var settlement = await Database.Settlements.FindAsync(te);
            if (settlement == null) return null;
            Database.Entry(settlement).Collection(s => s.Children).Load();
            return settlement;
        }

        public async Task<IEnumerable<Settlement>> GetAllAsync()
        {
            return await Database.Settlements.ToListAsync();
        }

        public async Task UpdateAsync(Settlement node)
        {
            try
            {
                Database.Entry(node).State = EntityState.Modified;
                await Database.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new UpdateOperationException($"Failed to update a node with TE= {node.Te}. " + ex.Message);
            }
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
