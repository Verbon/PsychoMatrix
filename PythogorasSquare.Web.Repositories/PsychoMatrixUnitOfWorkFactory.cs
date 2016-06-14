using PythogorasSquare.Web.Repositories.Interfaces;

namespace PythogorasSquare.Web.Repositories
{
    public class PsychoMatrixUnitOfWorkFactory : IPsychoMatrixUnitOfWorkFactory
    {
        public IPsychoMatrixUnitOfWork Create()
            => new PsychoMatrixUnitOfWork();
    }
}