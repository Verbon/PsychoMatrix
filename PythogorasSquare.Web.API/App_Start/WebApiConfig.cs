using System.Web.Http;

namespace PythogorasSquare.Web.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "rest/{controller}/{action}/{year}/{month}/{day}",
                defaults: new { year = RouteParameter.Optional, month = RouteParameter.Optional, day = RouteParameter.Optional }
            );
        }
    }
}