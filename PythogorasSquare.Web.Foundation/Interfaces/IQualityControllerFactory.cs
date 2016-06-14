using PythogorasSquare.Web.DomainModel;

namespace PythogorasSquare.Web.Foundation.Interfaces
{
    public interface IQualityControllerFactory
    {
        IQualityController CreateFrom(QualityDetailedInfo qualityDetailedInfo);
    }
}