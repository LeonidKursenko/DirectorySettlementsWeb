using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Exceptions
{
    public class CreateOperationException : InvalidOperationException
    {
        public CreateOperationException(string message) : base(message)
        {
        }
    }
}
