using DirectorySettlementsBLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.Interfaces
{
    /// <summary>
    /// IExportService provides interface for exporting filtered tree settlements to the document.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exports filtered tree settlements to the an array of bytes.
        /// </summary>
        /// <param name="settlements">Tree settlements.</param>
        /// <returns></returns>
        public byte[] Export(IEnumerable<SettlementDTO> settlements);
    }
}
