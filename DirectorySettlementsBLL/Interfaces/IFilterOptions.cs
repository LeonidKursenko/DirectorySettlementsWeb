using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.Interfaces
{
    /// <summary>
    /// IFilterOptions interface defines parameters for filtering the tree of settlements.
    /// </summary>
    public interface IFilterOptions
    {
        /// <value>Contains the name of settlements.</value>
        public string Name { get; set; }

        /// <value>Contains the type of settlements.</value>
        public string SettlementType { get; set; }
    }
}
