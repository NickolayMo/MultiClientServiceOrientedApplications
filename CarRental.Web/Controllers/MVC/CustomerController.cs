using CarRental.Web.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : ViewControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("account")]
        public ActionResult MyAccount()
        {
            return View();
        }
        [HttpGet]
        //define in RouteConf
        public ActionResult ReserveCar()
        {
            return View();
        }

        [HttpGet]
        [Route("reservations")]
        public ActionResult CurrentReservations()
        {
            return View();
        }
        [HttpGet]
        [Route("history")]
        public ActionResult RentalHistory()
        {
            return View();
        }
        
    }
}