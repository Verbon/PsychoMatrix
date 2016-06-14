using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PythogorasSquare.Repositories.Interfaces
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();

        Task<IReadOnlyCollection<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
    }
}