using System;
using System.Collections.Generic;
using PythogorasSquare.Repositories;
using PythogorasSquare.Repositories.Interfaces;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Repositories.Interfaces;
using PythogorasSquare.Web.Repositories.Repositories;

namespace PythogorasSquare.Web.Repositories
{
    public class PsychoMatrixUnitOfWork : UnitOfWork<PsychoMatrixContext>, IPsychoMatrixUnitOfWork
    {
        private IDictionary<Type, Type> _entityTypeToRepositoryTypeMappings;


        public IQualityDetailedInfoReadOnlyRepository QualityDetailedInfoRepository => (IQualityDetailedInfoReadOnlyRepository)GetRepository<QualityDetailedInfo>();


        public PsychoMatrixUnitOfWork()
        {
            InitializeRepositoryMapping();
        }


        protected override IReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>()
        {
            Type repositoryType;
            var repository = _entityTypeToRepositoryTypeMappings.TryGetValue(typeof(TEntity), out repositoryType)
                ? (ReadOnlyRepository<TEntity>)Activator.CreateInstance(repositoryType, Context)
                : base.CreateReadOnlyRepository<TEntity>();

            return repository;
        }


        private void InitializeRepositoryMapping()
        {
            _entityTypeToRepositoryTypeMappings = new Dictionary<Type, Type>
            {
                [typeof(QualityDetailedInfo)] = typeof(QualityDetailedInfoReadOnlyRepository)
            };
        }
    }
}