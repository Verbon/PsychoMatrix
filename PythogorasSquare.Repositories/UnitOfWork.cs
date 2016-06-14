using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PythogorasSquare.Repositories.Interfaces;

namespace PythogorasSquare.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : class, IDbContext, new()
    {
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;


        protected TContext Context { get; private set; }


        public UnitOfWork()
            : this(new TContext())
        {

        }

        public UnitOfWork(TContext context)
        {
            Context = context;
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }


        public IReadOnlyRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            object cachedRepository;
            if (_repositories.TryGetValue(typeof(TEntity), out cachedRepository))
            {
                return (IReadOnlyRepository<TEntity>)cachedRepository;
            }

            var readOnlyRepository = CreateReadOnlyRepository<TEntity>();
            _repositories.Add(typeof(TEntity), readOnlyRepository);

            return readOnlyRepository;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected virtual IReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>() where TEntity : class
            => new ReadOnlyRepository<TEntity>(Context);


        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Context?.Dispose();
            }

            Context = null;
            _disposed = true;
        }
    }
}