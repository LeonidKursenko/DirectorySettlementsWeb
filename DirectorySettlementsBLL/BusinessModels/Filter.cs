using DirectorySettlementsBLL.DTO;
using DirectorySettlementsBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.BusinessModels
{
    /// <summary>
    /// Filter class defines if the settlement is fits.
    /// </summary>
    public class Filter : IFilter
    {
        public bool IsTrue(SettlementDTO settlementDTO, IFilterOptions filterOptions)
        {
            if (string.IsNullOrWhiteSpace(filterOptions.Name) != true &&
                settlementDTO.Nu.Contains(filterOptions.Name) == false) return false;
            if (string.IsNullOrWhiteSpace(filterOptions.SettlementType) != true &&
                filterOptions.SettlementType.Equals(settlementDTO.Np) == false) return false;
            return true;
        }
    }
}
