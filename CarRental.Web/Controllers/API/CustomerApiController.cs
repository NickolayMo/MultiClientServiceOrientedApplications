using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Client.Entities;
using CarRental.Web.Core;
using CarRental.Web.Models;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    [Authorize]
    [UsesDisposableService]
    public class CustomerApiController : ApiControllerBase
    {
        private IAccountService _AccountService;

        [ImportingConstructor]
        public CustomerApiController(IAccountService service)
        {
            _AccountService = service;
        }
        protected override void RegisterServices(List<IServiceContract> dispоsableServices)
        {
            dispоsableServices.Add(_AccountService);
        }

        [HttpGet]
        [Route("account")]
        public HttpResponseMessage GetCustomerAccountInfo(HttpRequestMessage request)
        {
            return GetHttpResponse(request, () => {
                HttpResponseMessage response = null;
                Account account = _AccountService.GetCustomerAccountInfo(User.Identity.Name);
                response = request.CreateResponse(HttpStatusCode.OK, account);
                return response;
            });
        }
        [HttpPost]
        [Route("account")]
        public HttpResponseMessage UpdateCustomerAccountInfo(HttpRequestMessage request, [FromBody]Account accountModel)
        {
            return GetHttpResponse(request, ()=> {
                HttpResponseMessage response = null;
                ValidateAutorizedUser(accountModel.LoginEmail);
                List<string> errors = new List<string>();

                List<State> states = UIHelper.GetStates();
                State state = states.Where(item => item.Abbrev.ToUpper() == accountModel.State.ToUpper()).FirstOrDefault();
                if (state == null)
                {
                    errors.Add("Invalid state");
                }
                accountModel.ExpDate = accountModel.ExpDate.Substring(0, 2) + accountModel.ExpDate.Substring(3, 2);
                if (errors.Count == 0)
                {
                    _AccountService.UpdateCustomerAccountInfo(accountModel);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());
                }
                return response;
            });
           
        }
    }
}
