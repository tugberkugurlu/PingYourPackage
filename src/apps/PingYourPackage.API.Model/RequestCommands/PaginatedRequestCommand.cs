using PingYourPackage.API.Model.Validation;

namespace PingYourPackage.API.Model.RequestCommands {

    public class PaginatedRequestCommand : IRequestCommand {

        public PaginatedRequestCommand() { }
        public PaginatedRequestCommand(int page, int take) {

            Page = page;
            Take = take;
        }

        [Minimum(1)]
        public int Page { get; set; }

        [Minimum(1)]
        [Maximum(50)]
        public int Take { get; set; }
    }
}