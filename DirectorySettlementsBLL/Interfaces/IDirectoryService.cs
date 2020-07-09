using DirectorySettlementsBLL.BusinessModels;
using DirectorySettlementsBLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySettlementsBLL.Interfaces
{
    /// <summary>
    /// IDirectoryService provides CRUD interface.
    /// </summary>
    public interface IDirectoryService
    {
        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="node">A new node.</param>
        Task CreateAsync(SettlementDTO node);

        /// <summary>
        /// Deletes a node from the database.
        /// </summary>
        /// <param name="te">Te code of the node that deletes.</param>
        /// <param name="isCascade">Allows cascade delete of child nodes if it has "true" value.</param>
        Task DeleteAsync(string te, bool isCascade);

        /// <summary>
        /// Updates a node in the database.
        /// </summary>
        /// <param name="node">Node that needs to be updated.</param>
        /// <returns></returns>
        Task UpdateAsync(SettlementDTO node);

        /// <summary>
        /// Gets object by it`s TE code.
        /// </summary>
        /// <param name="te">Object TE code.</param>
        /// <returns>A node with specified TE code.</returns>
        Task<SettlementDTO> GetAsync(string te);

        /// <summary>
        /// Gets all direct children nodes.
        /// </summary>
        /// <param name="parentTe">TE code of the parent node.</param>
        /// <returns>All direct children nodes.</returns>
        Task<IEnumerable<SettlementDTO>> GetChildrenAsync(string parentTe);

        /// <summary>
        /// Gets all nodes.
        /// </summary>
        /// <returns>All nodes.</returns>
        Task<IEnumerable<SettlementDTO>> GetAllAsync();

        /// <summary>
        /// Gets all nodes that match the filter.
        /// </summary>
        /// <param name="filterOptions">Defines parameters for filter.</param>
        /// <returns>Filtered nodes.</returns>
        Task<IEnumerable<SettlementDTO>> FilterAsync(IFilterOptions filterOptions);
    }
}
