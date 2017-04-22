using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaxiForecast
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "Login", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "Home", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Home", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "Member", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Member", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "ViewPage1", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "ViewPage1", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "checkLogin", // Route name
                "{controller}/{action}/{username}/{password}", // URL with parameters
                new { controller = "Home", action = "checkLogin", username = UrlParameter.Optional, password = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "getDriverInfo", // Route name
                "{controller}/{action}/{carID}", // URL with parameters
                new { controller = "Home", action = "getDriverInfo", carID = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "ScoreBoard", // Route name
                "{controller}/{action}/{carID}", // URL with parameters
                new { controller = "Home", action = "ScoreBoard", carID = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "DirectionSettingBoard", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Home", action = "getDirectionSettingBoard"} // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}