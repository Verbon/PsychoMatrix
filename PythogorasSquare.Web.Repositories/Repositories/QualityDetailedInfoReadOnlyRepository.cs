using System.Data.Entity;
using System.Threading.Tasks;
using PythogorasSquare.Repositories;
using PythogorasSquare.Repositories.Interfaces;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Repositories.Interfaces;

namespace PythogorasSquare.Web.Repositories.Repositories
{
    internal class QualityDetailedInfoReadOnlyRepository : ReadOnlyRepository<QualityDetailedInfo>, IQualityDetailedInfoReadOnlyRepository
    {
        private readonly IDbContext _dbContext;


        public QualityDetailedInfoReadOnlyRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<QualityDetailedInfo> GetByPoweredQualityAsync(int qualityAssociatedDigit, int qualityPower)
            => await _dbContext.Set<QualityDetailedInfo>().SingleAsync(qdi => qdi.QualityPower == qualityPower && qdi.Quality.AssociatedDigit == qualityAssociatedDigit);
    }
}