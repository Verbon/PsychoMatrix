using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using PythogorasSquare.WebApp;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebActivator), nameof(UnityWebActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebActivator), nameof(UnityWebActivator.Shutdown))]

namespace PythogorasSquare.WebApp
{
    public static class UnityWebActivator
    {
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}