using System;
using System.Linq;
using PythogorasSquare.Common;
using PythogorasSquare.Foundation.Interfaces;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.Qualities;

namespace PythogorasSquare.Web.Foundation.Factories
{
    [UsedImplicitly]
    public class QualityEntityControllerFactory : IEntityControllerFactory<QualityDetailedInfo, IQualityController>
    {
        public IQualityController CreateFrom(QualityDetailedInfo qualityDetailedInfo)
        {
            var qualityName = qualityDetailedInfo.Quality.Name;
            var qualityPower = Enumerable.Repeat(qualityDetailedInfo.Quality.AssociatedDigit, qualityDetailedInfo.QualityPower)
                .Aggregate(String.Empty, (p, i) => p + i);
            var poweredQualityDescription = qualityDetailedInfo.QualityDescription.Description;

            return new QualityController(qualityName, qualityPower, poweredQualityDescription);
        }
    }
}