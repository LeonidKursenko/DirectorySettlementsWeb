using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.Exceptions
{
    /// <summary>
    /// ValidationException class generates when the data of the model is invalid.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <value>Stores name of invalid model property.</value>
        public string Property { get; protected set; }
        public ValidationException(string message, string property) : base(message)
        {

        }
    }
}
