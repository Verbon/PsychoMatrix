using System.Collections.Generic;
using System.Linq;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Foundation.Interfaces;

namespace PythogorasSquare.Web.Foundation.Providers
{
    public class CachingQualityControllerProvider : IQualityControllerProvider
    {
        private readonly IQualityControllerFactory _qualityControllerFactory;

        private readonly Dictionary<QualityDetailedInfo, IQualityController> _qualityControllers;


        public CachingQualityControllerProvider(
            IQualityControllerFactory qualityControllerFactory,
            IEqualityComparer<QualityDetailedInfo> qualityDetailedInfoEqualityComparer)
        {
            _qualityControllerFactory = qualityControllerFactory;

            _qualityControllers = new Dictionary<QualityDetailedInfo, IQualityController>(qualityDetailedInfoEqualityComparer);
        }


        public IQualityController GetControllerFor(QualityDetailedInfo qualityDetailedInfo)
        {
            IQualityController qualityController;
            if (_qualityControllers.TryGetValue(qualityDetailedInfo, out qualityController))
            {
                return qualityController;
            }

            qualityController = _qualityControllerFactory.CreateFrom(qualityDetailedInfo);
            _qualityControllers.Add(qualityDetailedInfo, qualityController);

            return qualityController;
        }

        public QualityDetailedInfo GetEntityOf(IQualityController qualityController)
        {
            return _qualityControllers.Single(pair => pair.Value == qualityController).Key;
        }
    }
}