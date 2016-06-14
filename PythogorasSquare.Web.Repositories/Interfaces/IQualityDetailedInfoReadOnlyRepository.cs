using System.Threading.Tasks;
using PythogorasSquare.Repositories.Interfaces;
using PythogorasSquare.Web.DomainModel;

namespace PythogorasSquare.Web.Repositories.Interfaces
{
    public interface IQualityDetailedInfoReadOnlyRepository : IReadOnlyRepository<QualityDetailedInfo>
    {
        Task<QualityDetailedInfo> GetByPoweredQualityAsync(int qualityAssociatedDigit, int qualityPower);
    }
}