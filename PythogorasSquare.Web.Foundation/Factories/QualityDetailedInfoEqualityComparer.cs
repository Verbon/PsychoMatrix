using System.Collections.Generic;
using PythogorasSquare.Common;
using PythogorasSquare.Web.DomainModel;

namespace PythogorasSquare.Web.Foundation.Factories
{
    [UsedImplicitly]
    public class QualityDetailedInfoEqualityComparer : IEqualityComparer<QualityDetailedInfo>
    {
        public bool Equals(QualityDetailedInfo x, QualityDetailedInfo y)
            => x.QualityId == y.QualityId && x.QualityPower == y.QualityPower;

        public int GetHashCode(QualityDetailedInfo obj)
            => obj.QualityId.GetHashCode() ^ obj.QualityPower.GetHashCode();
    }
}