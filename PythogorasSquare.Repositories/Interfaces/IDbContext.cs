using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PythogorasSquare.Repositories.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}