using System.Web.Http;

namespace PythogorasSquare.Web.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "rest/{controller}/{action}/{year}/{month}/{day}",
                new { year = RouteParameter.Optional, month = RouteParameter.Optional, day = RouteParameter.Optional }
            );
        }
    }
}