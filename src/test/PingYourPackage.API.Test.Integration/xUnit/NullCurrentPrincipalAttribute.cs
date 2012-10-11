using System.Reflection;
using System.Threading;
using Xunit;

namespace PingYourPackage.API.Test.Integration {

    public class NullCurrentPrincipalAttribute : BeforeAfterTestAttribute {

        public override void Before(MethodInfo methodUnderTest) {

            Thread.CurrentPrincipal = null;
        }
    }
}
