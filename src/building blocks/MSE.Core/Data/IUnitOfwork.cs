using System.Threading.Tasks;

namespace MSE.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> commit();
    }
}
