using System.Collections.Generic;
using System.Linq;
using PythogorasSquare.Common;
using PythogorasSquare.Foundation.Interfaces;

namespace PythogorasSquare.Foundation.Providers
{
    [UsedImplicitly]
    public class CachingEntityControllerProvider<TEntiy, TController> : IEntityControllerProvider<TEntiy, TController>
    {
        private readonly IEntityControllerFactory<TEntiy, TController> _entityControllerFactory;

        private readonly Dictionary<TEntiy, TController> _controllers;


        public CachingEntityControllerProvider(
            IEntityControllerFactory<TEntiy, TController> entityControllerFactory,
            IEqualityComparer<TEntiy> entityEqualityComparer)
        {
            _entityControllerFactory = entityControllerFactory;

            _controllers = new Dictionary<TEntiy, TController>(entityEqualityComparer);
        }


        public TController GetControllerFor(TEntiy entity)
        {
            TController controller;
            if(_controllers.TryGetValue(entity, out controller))
            {
                return controller;
            }

            controller = _entityControllerFactory.CreateFrom(entity);
            _controllers.Add(entity, controller);

            return controller;
        }

        public TEntiy GetEntityOf(TController controller)
            => _controllers.Single(pair => pair.Value.Equals(controller)).Key;
    }
}