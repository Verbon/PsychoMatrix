using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PythogorasSquare.Repositories.Interfaces;

namespace PythogorasSquare.Repositories
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        private readonly IDbContext _dbContext;


        public ReadOnlyRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
            => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IReadOnlyCollection<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }
}