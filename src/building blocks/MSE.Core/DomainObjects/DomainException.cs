using System;
using System.Collections.Generic;
using System.Text;

namespace MSE.Core.DomainObjects
{
    public class DomainException : Exception
    {

        public DomainException()
        {

        }

        public DomainException(string message)
            : base(message)
        {

        }

        public DomainException(string message, Exception innException)
            : base(message, innException)
        {

        }
    }
}
