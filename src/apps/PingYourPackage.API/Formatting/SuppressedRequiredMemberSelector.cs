using System.Net.Http.Formatting;
using System.Reflection;

namespace PingYourPackage.API.Formatting {

    public class SuppressedRequiredMemberSelector
        : IRequiredMemberSelector {

        public bool IsRequiredMember(MemberInfo member) {

            return false;
        }
    }
}