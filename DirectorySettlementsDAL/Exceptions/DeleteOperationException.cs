using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDAL.Exceptions
{
    public class DeleteOperationException : InvalidOperationException
    {
        public DeleteOperationException(string message) : base(message)
        {
        }
    }
}
