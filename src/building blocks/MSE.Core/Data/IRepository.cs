using MSE.Core.DomainObjects;
using System;

namespace MSE.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {

    }


}
