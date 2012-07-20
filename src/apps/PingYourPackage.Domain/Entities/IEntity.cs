using System;

namespace PingYourPackage.Domain.Entities {

    public interface IEntity {

        Guid Key { get; set; }
    }
}