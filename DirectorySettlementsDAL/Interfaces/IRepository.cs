using System;
using System.Collections.Generic;
using System.Text;

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
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets object by it`s TE code.
        /// </summary>
        /// <param name="te">Object TE code.</param>
        /// <returns></returns>
        T Get(string te);

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
        void Create(T node);

        /// <summary>
        /// Updates a node in the database.
        /// </summary>
        /// <param name="node">Node that needs to be updated.</param>
        void Update(T node);

        /// <summary>
        /// Deletes a node from the database.
        /// </summary>
        /// <param name="te">Te code of the node that deletes.</param>
        void Delete(string te);

        /// <summary>
        /// Deletes a node from the database with all children nodes.
        /// </summary>
        /// <param name="te">Te code of the node that deletes.</param>
        void DeleteAll(string te);

        /// <summary>
        /// Deletes all nodes from the database.
        /// </summary>
        void Clear();
    }
}
