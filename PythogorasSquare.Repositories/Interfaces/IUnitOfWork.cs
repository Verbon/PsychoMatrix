using System;
using System.Threading.Tasks;

namespace PythogorasSquare.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IReadOnlyRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task SaveChangesAsync();
    }
}