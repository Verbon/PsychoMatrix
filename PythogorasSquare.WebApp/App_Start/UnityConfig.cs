using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using PythogorasSquare.Foundation.Interfaces;
using PythogorasSquare.Foundation.Providers;
using PythogorasSquare.Web.DomainModel;
using PythogorasSquare.Web.Foundation.Factories;
using PythogorasSquare.Web.Foundation.Interfaces;
using PythogorasSquare.Web.Foundation.PsychoMatrix;
using PythogorasSquare.Web.Repositories;
using PythogorasSquare.Web.Repositories.Interfaces;

namespace PythogorasSquare.WebApp
{
    public class UnityConfig
    {
        private readonly static Lazy<IUnityContainer> LazyContainer;


        static UnityConfig()
        {
            LazyContainer = new Lazy<IUnityContainer>(CreateLazyContainer);
        }


        public static IUnityContainer GetConfiguredContainer()
        {
            return LazyContainer.Value;
        }


        private static IUnityContainer CreateLazyContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IPsychoMatrixUnitOfWorkFactory, PsychoMatrixUnitOfWorkFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEntityControllerFactory<QualityDetailedInfo, IQualityController>, QualityEntityControllerFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEqualityComparer<QualityDetailedInfo>, QualityDetailedInfoEqualityComparer>(new ContainerControlledLifetimeManager());

            container.RegisterType<IEntityControllerProvider<QualityDetailedInfo, IQualityController>, CachingEntityControllerProvider<QualityDetailedInfo, IQualityController>>(new ContainerControlledLifetimeManager());

            container.RegisterType<IPsychoMatrixService, PsychoMatrixService>(new ContainerControlledLifetimeManager());

        }
    }
}