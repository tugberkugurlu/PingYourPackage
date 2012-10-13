using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace PingYourPackage.API.ModelBinding {

    public class PrincipalParameterBinding : HttpParameterBinding {

        public PrincipalParameterBinding(
            HttpParameterDescriptor descriptor)
            : base(descriptor) {
        }

        public override Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider, 
            HttpActionContext actionContext, 
            CancellationToken cancellationToken) {

            string name = Descriptor.ParameterName;
            IPrincipal principal = Thread.CurrentPrincipal;
            actionContext.ActionArguments.Add(name, principal);

            return Task.FromResult(0);
        }
    }
}