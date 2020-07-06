using System;
using System.Collections.Generic;

namespace DirectorySettlementsDAL.Entities
{
    /// <summary>
    /// InitialTable class uses for the initialization.
    /// </summary>
    public partial class InitialTable
    {
        /// <value>COATUU object code (код об`єкта КОАТУУ). It`s a unique id.</value>
        public string Te { get; set; }

        /// <value>Object category.</value>
        public string Np { get; set; }

        /// <value>The name of the object in Ukrainian.</value>
        public string Nu { get; set; }
    }
}
