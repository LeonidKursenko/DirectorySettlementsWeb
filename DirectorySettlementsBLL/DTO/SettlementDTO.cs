using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DirectorySettlementsBLL.DTO
{
    /// <summary>
    /// Settlement class contains all information about settlement.
    /// </summary>
    public class SettlementDTO
    {
        public SettlementDTO()
        {
            Children = new HashSet<SettlementDTO>();
        }

        /// <value>COATUU object code (код об`єкта КОАТУУ). It`s a unique id.</value>
        public string Te { get; set; }

        /// <value>Object category.</value>
        public string Np { get; set; }

        /// <value>The name of the object in Ukrainian.</value>
        public string Nu { get; set; }

        /// <value>ParentId contains Te of a parent node.</value>
        public string ParentId { get; set; }

        /// <value>Parent node.</value>
        //[JsonIgnore]
        //public virtual SettlementDTO Parent { get; set; }

        /// <value>Contains a collection of child nodes.</value>
        public virtual ICollection<SettlementDTO> Children { get; set; }

        public bool HasChildrenNode { get; set; }
    }
}
