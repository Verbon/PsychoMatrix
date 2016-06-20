using System.Collections.Generic;
using PythogorasSquare.Clients.ServiceClients.DataContracts;
using PythogorasSquare.Common;

namespace PythogorasSquare.Clients.Foundation.Factories
{
    [UsedImplicitly]
    public class ServiceQualityEqualityComparer : IEqualityComparer<ServiceQuality>
    {
        public bool Equals(ServiceQuality x, ServiceQuality y)
            => x.Name == y.Name && x.Power == y.Power;

        public int GetHashCode(ServiceQuality obj)
            => obj.Name.GetHashCode() ^ obj.Power.GetHashCode();
    }
}