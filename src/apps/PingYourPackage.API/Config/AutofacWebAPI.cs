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

            config.DependencyResolver = 
                new AutofacWebApiDependencyResolver(
                    RegisterServices(new ContainerBuilder()));
        }

        private static IContainer RegisterServices(
            ContainerBuilder builder) {

            builder.RegisterAssemblyTypes(
                Assembly.GetExecutingAssembly()).PropertiesAutowired();

            //EF DbContext
            builder.RegisterType<EntitiesContext>()
                .As<DbContext>().InstancePerApiRequest();

            //Repositories
            builder.RegisterType<EntityRepository<User>>()
                .As<IEntityRepository<User>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<Role>>()
                .As<IEntityRepository<Role>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<UserInRole>>()
                .As<IEntityRepository<UserInRole>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<Affiliate>>()
                .As<IEntityRepository<Affiliate>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<Shipment>>()
                .As<IEntityRepository<Shipment>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<ShipmentType>>()
                .As<IEntityRepository<ShipmentType>>()
                .InstancePerApiRequest();

            builder.RegisterType<EntityRepository<ShipmentState>>()
                .As<IEntityRepository<ShipmentState>>()
                .InstancePerApiRequest();

            //services
            builder.RegisterType<CryptoService>()
                .As<ICryptoService>()
                .InstancePerApiRequest();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder.Build();
        }
    }
}