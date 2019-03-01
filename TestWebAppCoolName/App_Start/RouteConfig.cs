using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestWebAppCoolName
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Course",
                url: "kurz/{title}",
                defaults: new { controller = "Course", action = "Index" }
              
            );
            routes.MapRoute(
                name: "BlogANew",
                url: "blog/new",
                defaults: new { controller = "Blog", action = "New" }

            );
            routes.MapRoute(
                name: "BlogAdmin",
                url: "blogAdmin",  
                defaults: new { controller = "Blog", action = "BlogAdmin" }

            );
            routes.MapRoute(
                name: "Blog",
                url: "blog/{title}",
                defaults: new { controller = "Blog", action = "Index" }

            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
