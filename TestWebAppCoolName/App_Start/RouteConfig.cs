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
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "CourseAdmin",
                url: "kurz/admin",
                defaults: new { controller = "Course", action = "Admin" }

            );  
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
                url: "Blog/Admin",
                defaults: new { controller = "Blog", action = "Admin" }

            );
            routes.MapRoute(
                name: "Blog",
                url: "blog/{title}",
                defaults: new { controller = "Blog", action = "Index" }
            );
            routes.MapRoute(
                name: "BlogIndex",
                url: "blog/",
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
