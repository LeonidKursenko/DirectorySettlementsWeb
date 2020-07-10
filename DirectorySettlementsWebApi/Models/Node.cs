using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorySettlementsWebApi.Models
{
    public class Node
    {
        public Node()
        {
            Children = new HashSet<Node>();
        }

        /// <value>COATUU object code (код об`єкта КОАТУУ). It`s a unique id.</value>
        public string Te { get; set; }

        /// <value>Object category.</value>
        public string Np { get; set; }

        /// <value>The name of the object in Ukrainian.</value>
        public string Nu { get; set; }

        /// <value>ParentId contains Te of a parent node.</value>
        public string ParentId { get; set; }

        /// <value>Contains a collection of child nodes.</value>
        public virtual ICollection<Node> Children { get; set; }
    }
}
