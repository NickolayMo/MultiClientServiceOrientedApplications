using CarRental.Web.Core;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace CarRental.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("home")]
    public class HomeController : ViewControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("my")]
        [Authorize]
        public ActionResult MyAccount()
        {
            return View();
        }
    }
}