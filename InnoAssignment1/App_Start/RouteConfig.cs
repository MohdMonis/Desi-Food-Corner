using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InnoAssignment1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Enabling Attribute Routing   
            //routes.MapMvcAttributeRoutes();

            // Default routing
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
                );

            // Custom routing
            // {after adding this code the related routing attribute should be added to the respactive components}
            // (   [Route("Home/ContactUs")]   )

            //routes.MapRoute(
            //      name: "Profile",
            //      url: "{controller}/{action}/{id,code}",
            //      defaults: new { controller = "Order", action = "UpdateStatus", id = UrlParameter.Optional, code = UrlParameter.Optional }
            //    );
        }
    }
}

