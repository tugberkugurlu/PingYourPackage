using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Config {

    public class AutofacWebAPI {

        public static void Initialize(HttpConfiguration config) {

            Initialize(config, 
                RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container) {

            config.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder) {

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // EF DbContext
            builder.RegisterType<EntitiesContext>()
                   .As<DbContext>()
                   .InstancePerApiRequest();

            // Register repositories by using Autofac's OpenGenerics feature
            // More info: http://code.google.com/p/autofac/wiki/OpenGenerics
            builder.RegisterGeneric(typeof(EntityRepository<>))
                   .As(typeof(IEntityRepository<>))
                   .InstancePerApiRequest();

            // Services
            builder.RegisterType<CryptoService>()
                .As<ICryptoService>()
                .InstancePerApiRequest();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerApiRequest();

            builder.RegisterType<ShipmentService>()
                .As<IShipmentService>()
                .InstancePerApiRequest();

            return builder.Build();
        }
    }
}