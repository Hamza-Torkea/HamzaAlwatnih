using System.Web.Mvc;
using System.Web.Routing;

namespace alwatnia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}/{title}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional
            }, new string[1] { "alwatnia.Controllers" });

            routes.MapRoute("OnlyAction", "{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
                title = UrlParameter.Optional
            }, new string[1] { "alwatnia.Controllers" });
        }
    }
}