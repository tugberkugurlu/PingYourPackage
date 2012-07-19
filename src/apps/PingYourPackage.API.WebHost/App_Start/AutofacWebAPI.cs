using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.API.WebHost {

    public class AutofacWebAPI {

        public static void Initialize(HttpConfiguration config) {

            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                RegisterServices(new ContainerBuilder())
            );
        }

        private static IContainer RegisterServices(ContainerBuilder builder) {

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            //EF DbContext
            builder.RegisterType<EntitiesContext>()
                .As<IEntitiesContext>().InstancePerApiRequest();

            //Repositories
            builder.RegisterType<EntityRepository<User>>()
                .As<IEntityRepository<User>>().InstancePerApiRequest();
            builder.RegisterType<EntityRepository<Role>>()
                .As<IEntityRepository<Role>>().InstancePerApiRequest();

            return builder.Build();
        }
    }
}