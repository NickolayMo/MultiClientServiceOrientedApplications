using CarRental.Web.Core;
using CarRental.Web.Models;
using CarRental.Web.Services;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/account")]
    public class AccountApiController : ApiControllerBase
    {
        private ISecurityAdapter _securityAdapter;

        public AccountApiController(ISecurityAdapter securityAdapter)
        {
            _securityAdapter = securityAdapter;

        }

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
    }
}
