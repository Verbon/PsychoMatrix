using PythogorasSquare.Web.DomainModel;

namespace PythogorasSquare.Web.Foundation.Interfaces
{
    public interface IQualityControllerProvider
    {
        IQualityController GetControllerFor(QualityDetailedInfo qualityDetailedInfo);

        QualityDetailedInfo GetEntityOf(IQualityController qualityController);
    }
}