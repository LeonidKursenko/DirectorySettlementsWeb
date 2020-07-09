using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Exceptions
{
    public class UpdateOperationException : Exception
    {
        public UpdateOperationException(string message) : base(message)
        {
        }
    }
}
