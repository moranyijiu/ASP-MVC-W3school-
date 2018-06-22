using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Catalog",
                url: "{identity}.html",
                defaults: new { controller = "Home", action = "Catalog" }
            );

            routes.MapRoute(
               name: "Course",
               url: "{catalogidentity}/{identity}.html",
               defaults: new { controller = "Home", action = "Course" }
           );

            routes.MapRoute(
               name: "Article",
               url: "{catalogidentity}/{courseidentity}/{identity}.html",
               defaults: new { controller = "Home", action = "Article" }
            );

            routes.MapRoute(
               name: "Login",
               url: "admin/{path}",
               defaults: new { controller = "Account", action = "Login" }
            );
  
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
