using System;
using System.Collections.Generic;

namespace DirectorySettlementsDAL.Entities
{
    public partial class Settlement
    {
        /// <summary>
        /// Settlement class contains all information about settlement.
        /// </summary>
        public Settlement()
        {
            Children = new HashSet<Settlement>();
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
        public virtual Settlement Parent { get; set; }

        /// <value>Contains a collection of child nodes.</value>
        public virtual ICollection<Settlement> Children { get; set; }
    }
}
