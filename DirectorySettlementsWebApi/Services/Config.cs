using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorySettlementsWebApi.Services
{
    /// <summary>
    /// Config class contains configuration information.
    /// </summary>
    public class Config
    {
        /// <value>Gets and sets database connection string.</value>
        public static string DbConnection { get; set; }
    }
}
