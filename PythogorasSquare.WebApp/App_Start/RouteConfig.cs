using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace PythogorasSquare.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "PsychoMatrix" }
            );

            routes.MapRoute(
                name: "DefaultApi",
                url: "api/{controller}/{action}/{year}/{month}/{day}",
                defaults: new { controller = "pythogoras", action = "psychoMatrix", year = DateTime.UtcNow.Year, month = DateTime.UtcNow.Month, day = DateTime.UtcNow.Day });
        }
    }
}