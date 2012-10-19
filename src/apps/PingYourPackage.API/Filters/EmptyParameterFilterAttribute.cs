using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;

namespace PingYourPackage.API.Filters {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class EmptyParameterFilterAttribute : ActionFilterAttribute {

        public string ParameterName { get; private set; }

        public EmptyParameterFilterAttribute(string parameterName) {

            if (string.IsNullOrEmpty(parameterName)) {

                throw new ArgumentNullException("parameterName");
            }

            ParameterName = parameterName;
        }

        public override void OnActionExecuting(
            HttpActionContext actionContext) {
            
            object parameterValue;
            if (actionContext.ActionArguments.TryGetValue(ParameterName, out parameterValue)) {

                if (parameterValue == null) {

                    actionContext.ModelState.AddModelError(
                        ParameterName, FormatErrorMessage(ParameterName));

                    actionContext.Response = actionContext
                        .Request.CreateErrorResponse(
                            HttpStatusCode.BadRequest, actionContext.ModelState);
                }
            }
        }

        private string FormatErrorMessage(string parameterName) {

            return string.Format("The {0} cannot be null.", parameterName);
        }
    }
}