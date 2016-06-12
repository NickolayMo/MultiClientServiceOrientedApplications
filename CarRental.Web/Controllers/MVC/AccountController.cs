using CarRental.Web.Core;
using CarRental.Web.Models;
using CarRental.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CarRental.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("account")]
    public class AccountController : ViewControllerBase
    {
        
        private ISecurityAdapter _adapter;

        [ImportingConstructor]
        public AccountController(ISecurityAdapter adapter)
        {
            _adapter = adapter;
        }
        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            _adapter.Initialize();

            return View(new AccountLoginModel() { ReturnUrl = returnUrl});
        }
        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Register()
        {
            _adapter.Initialize();
            return View();
        }
    }
}