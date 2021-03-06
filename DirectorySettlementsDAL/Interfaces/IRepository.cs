﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DirectorySettlementsDAL.Interfaces
{
    /// <summary>
    /// IRepository interface implements CRUD interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all nodes.
        /// </summary>
        /// <returns>All nodes.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets object by it`s TE code.
        /// </summary>
        /// <param name="te">Object TE code.</param>
        /// <returns></returns>
        Task<T> GetAsync(string te);

        /// <summary>
        /// Finds nodes that match the condition.
        /// </summary>
        /// <param name="predicate">Condition.</param>
        /// <returns>Nodes that matches the condition.</returns>
        IEnumerable<T> Find(Func<T, Boolean> predicate);

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="node">A new node.</param>
        Task CreateAsync(T node);

        /// <summary>
        /// Adds range of nodes.
        /// </summary>
        /// <param name="nodes">Range of nodes.</param>
        Task AddRangeAsync(IEnumerable<T> nodes);

        /// <summary>
        /// Updates a node in the database.
        /// </summary>
        /// <param name="node">Node that needs to be updated.</param>
        Task UpdateAsync(T node);

        /// <summary>
        /// Deletes a node from the database.
        /// </summary>
        /// <param name="te">Te code of the node that deletes.</param>
        Task DeleteAsync(string te);

        /// <summary>
        /// Deletes a node from the database with all children nodes.
        /// </summary>
        /// <param name="te">Te code of the node that deletes.</param>
        Task DeleteAllAsync(string te);

        /// <summary>
        /// Deletes all nodes from the database.
        /// </summary>
        Task ClearAsync();
    }
}
