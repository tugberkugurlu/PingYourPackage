using PingYourPackage.API.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PingYourPackage.API.Test.Filters {
    
    public class EmptyParameterFilterAttributeTest {

        [Fact]
        public void Sets_400_Response_If_Indicated_Parameter_Is_Null() {

            //Arange
            var parameterName = "requestModel";
            var emptyParameterFilter = new EmptyParameterFilterAttribute(parameterName);
            var request = new HttpRequestMessage();
            var actionContext = ContextUtil.GetHttpActionContext(request);
            actionContext.ActionArguments[parameterName] = null;

            //Act
            emptyParameterFilter.OnActionExecuting(actionContext);

            //Assert
            Assert.NotNull(actionContext.Response);
            Assert.Equal(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        [Fact]
        public void Does_Not_Set_Response_If_Indicated_Parameter_Is_Not_Null() {

            //Arange
            var parameterName = "requestModel";
            var emptyParameterFilter = new EmptyParameterFilterAttribute(parameterName);
            var request = new HttpRequestMessage();
            var actionContext = ContextUtil.GetHttpActionContext(request);
            actionContext.ActionArguments[parameterName] = new object();

            //Act
            emptyParameterFilter.OnActionExecuting(actionContext);

            //Assert
            Assert.Null(actionContext.Response);
        }
    }
}