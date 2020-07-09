using DirectorySettlementsBLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.Interfaces
{
    /// <summary>
    /// IFilter interface defines if the settlement is fits.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Defines if the settlement is fits.
        /// </summary>
        /// <param name="settlementDTO"></param>
        /// <param name="filterOptions"></param>
        /// <returns>Returns "true" if the settlement is fits.</returns>
        public bool IsTrue(SettlementDTO settlementDTO, IFilterOptions filterOptions);
    }
}
