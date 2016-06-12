using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarRental.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "accountRegisterRoot",
                url: "account/register",
                defaults: new { controller = "Account", action = "Register" }
            );
            routes.MapRoute(
               name: "accountRegister",
               url: "account/register/{*catchcall}",
               defaults: new { controller = "Account", action = "Register" }
           );

            routes.MapRoute(
                name: "reserveCarRoot",
                url: "customer/reserve",
                defaults: new { controller = "Customer", action = "ReserveCar" }
            );
            routes.MapRoute(
               name: "reserveCar",
               url: "customer/reserve/{*catchcall}",
               defaults: new { controller = "Customer", action = "ReserveCar" }
           );
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
