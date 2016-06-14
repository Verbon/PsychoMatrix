using System.Collections.Generic;
using PythogorasSquare.Web.DomainModel;

namespace PythogorasSquare.Web.Foundation.Qualities
{
    public class QualityDetailedInfoEqualityComparer : IEqualityComparer<QualityDetailedInfo>
    {
        public bool Equals(QualityDetailedInfo x, QualityDetailedInfo y)
            => x.QualityId == y.QualityId && x.QualityPower == y.QualityPower;

        public int GetHashCode(QualityDetailedInfo obj)
            => obj.QualityId.GetHashCode() ^ obj.QualityPower.GetHashCode();
    }
}