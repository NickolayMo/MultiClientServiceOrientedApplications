using CarRental.Web.Core;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CarRental.Web.Controllers.API
{
    public class UsesDisposableServiceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IServiceAwareController controller = actionContext.ControllerContext.Controller as IServiceAwareController;
            if (controller != null)
            {
                controller.RegisterDisposableServices(((IServiceAwareController)controller).DisposableServices);
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            IServiceAwareController controller = actionExecutedContext.ActionContext.ControllerContext.Controller as IServiceAwareController;
            if (controller != null)
            {
                foreach (var service in controller.DisposableServices)
                {
                    if (service != null && service is IDisposable)
                    {
                        (service as IDisposable).Dispose();
                    }

                }
            }
        }
    }
}