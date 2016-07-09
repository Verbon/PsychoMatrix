using PythogorasSquare.Common;
using PythogorasSquare.Web.Repositories.Interfaces;

namespace PythogorasSquare.Web.Repositories
{
    [UsedImplicitly]
    public class PsychoMatrixUnitOfWorkFactory : IPsychoMatrixUnitOfWorkFactory
    {
        public IPsychoMatrixUnitOfWork Create()
            => new PsychoMatrixUnitOfWork();
    }
}