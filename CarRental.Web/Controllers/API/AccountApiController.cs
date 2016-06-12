using CarRental.Web.Core;
using CarRental.Web.Models;
using CarRental.Web.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace CarRental.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    public class AccountApiController : ApiControllerBase
    {
        private ISecurityAdapter _securityAdapter;

        [ImportingConstructor]
        public AccountApiController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;

        }

        [HttpPost]
        [Route("login/confirm")]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody]AccountLoginModel accountModel)
        {
            HttpResponseMessage response = null;
            return GetHttpResponse(request, () =>{
                bool login = _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, accountModel.RememberMe);
                if (login)
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.Unauthorized, "Login fail");
                }
                return response;
            });
           
        }
        [HttpPost]
        [Route("register/validate1")]
        public HttpResponseMessage ValidateRegistrationStep1(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () => 
            {
                HttpResponseMessage response = null;
                List<string> errors = new List<string>();
                List<State> states = UIHelper.GetStates();
                State state = states.FirstOrDefault(item => item.Abbrev.ToUpper() == accountModel.State.ToUpper());
                if (state == null)
                {
                    errors.Add("Invalid state");
                }
                if (errors.Count == 0)
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());
                }
                return response;
            });
        }
        [HttpPost]
        [Route("register/validate2")]
        public HttpResponseMessage ValidateRegistrationStep2(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () => {
                HttpResponseMessage response = null;
                if (!_securityAdapter.UserExists(accountModel.LoginEmail))
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse<string[]>(HttpStatusCode.BadRequest, new List<string>() { "Account with this login email already exists" }.ToArray());
                }
                return response;
            });
        }
        [HttpPost]
        [Route("register/validate3")]
        public HttpResponseMessage ValidateRegistrationStep3(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, () => {
                HttpResponseMessage response = null;
                List<string> errors = new List<string>();
                if (accountModel.CreditCart.Length < 16)
                {
                    errors.Add("Credit cart number is invalid format");
                }
                Match matchExpDate = Regex.Match(accountModel.ExpDate, @"(0[1-9]|1[0-2])\/[0-9]{2}", RegexOptions.IgnoreCase);
                if (!matchExpDate.Success)
                {
                    errors.Add("Expiration date is invalid");
                }
                if (errors.Count == 0)
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    response = request.CreateResponse<string[]>(HttpStatusCode.BadGateway, errors.ToArray());
                }
                return response;
            });
        }
        [HttpPost]
        [Route("register/createAccount")]
        public HttpResponseMessage CreateAccount(HttpRequestMessage request, [FromBody]AccountRegisterModel accountModel)
        {
            return GetHttpResponse(request, ()=> {
                HttpResponseMessage response = null;
                if (ValidateRegistrationStep1(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep2(request, accountModel).IsSuccessStatusCode &&
                    ValidateRegistrationStep3(request, accountModel).IsSuccessStatusCode)
                {
                    _securityAdapter.Register(accountModel.LoginEmail, accountModel.Password,
                        propertyValues: new
                        {
                            FirstName = accountModel.FirstName,
                            LastName = accountModel.LastName,
                            Address = accountModel.Address,
                            City = accountModel.City,
                            State = accountModel.State,
                            ZipCode = accountModel.ZipCode,
                            CreditCart = accountModel.CreditCart,
                            ExpDate = accountModel.ExpDate
                        });
                    _securityAdapter.Login(accountModel.LoginEmail, accountModel.Password, false);
                    response = request.CreateResponse(HttpStatusCode.OK);
                }
                return response;
            });
        }
    }
}
