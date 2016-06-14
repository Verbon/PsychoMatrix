using PythogorasSquare.Repositories.Interfaces;

namespace PythogorasSquare.Web.Repositories.Interfaces
{
    public interface IPsychoMatrixUnitOfWork : IUnitOfWork
    {
        IQualityDetailedInfoReadOnlyRepository QualityDetailedInfoRepository { get; }
    }
}