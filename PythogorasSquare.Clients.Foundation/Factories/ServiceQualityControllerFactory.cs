using PythogorasSquare.Clients.Foundation.Interfaces;
using PythogorasSquare.Clients.ServiceClients.DataContracts;
using PythogorasSquare.Common;
using PythogorasSquare.Foundation.Interfaces;

namespace PythogorasSquare.Clients.Foundation.Factories
{
    [UsedImplicitly]
    public class ServiceQualityControllerFactory : IEntityControllerFactory<ServiceQuality, IQualityController>
    {
        public IQualityController CreateFrom(ServiceQuality serviceEntity)
        {
            var qualityController = new QualityController(serviceEntity.Name, serviceEntity.Power, serviceEntity.Description);

            return qualityController;
        }
    }
}