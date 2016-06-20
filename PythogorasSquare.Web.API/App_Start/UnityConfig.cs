using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Web.Http;
using PythogorasSquare.Foundation.Interfaces;
using PythogorasSquare.Foundation.Providers;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Foundation.Factories;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.PsychoMatrix;
using PythogorasSquare.Web.Foundation.Qualities;
using PythogorasSquare.Web.Repositories;
using PythogorasSquare.Web.Repositories.Interfaces;
using Unity.WebApi;

namespace PythogorasSquare.Web.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            ConfigureContainer(container);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }


        private static void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<IPsychoMatrixUnitOfWorkFactory, PsychoMatrixUnitOfWorkFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEntityControllerFactory<QualityDetailedInfo, IQualityController>, QualityEntityControllerFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEqualityComparer<QualityDetailedInfo>, QualityDetailedInfoEqualityComparer>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEntityControllerProvider<QualityDetailedInfo, IQualityController>, CachingEntityControllerProvider<QualityDetailedInfo, IQualityController>>(new ContainerControlledLifetimeManager());

            container.RegisterType<IPsychoMatrixService, PsychoMatrixService>(new ContainerControlledLifetimeManager());
        }
    }
}