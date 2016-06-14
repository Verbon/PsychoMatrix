using System;
using System.Linq;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.Qualities;

namespace PythogorasSquare.Web.Foundation.Factories
{
    public class QualityControllerFactory : IQualityControllerFactory
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