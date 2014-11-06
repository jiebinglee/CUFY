using Autofac;
using ChinaUnicom.Fuyang.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using ChinaUnicom.Fuyang.Framework.Data;
using ChinaUnicom.Fuyang.CreditManagement;
using ChinaUnicom.Fuyang.Framework.Adapter;

namespace ChinaUnicom.Fuyang.WebService
{
    public static class AutofacContainer
    {
        static IContainer _currentContainer;

        /// <summary>
        /// Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        public static ILifetimeScope Current
        {
            get
            {
                return _currentContainer.BeginLifetimeScope();
            }
        }

        public static void ConfigureContainer()
        {
            /*
             * Add here the code configuration or the call to configure the container 
             * using the application configuration file
             */

            var bulider = new ContainerBuilder();

            var baseType = typeof(IDependency);
            var assemblys = AssemblyLocator.GetAssemblies();//AppDomain.CurrentDomain.GetAssemblies().ToList();
            var AllServices = assemblys
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p != baseType);
            bulider.RegisterAssemblyTypes(assemblys.ToArray())
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .AsImplementedInterfaces();

            bulider.RegisterModule(new DataModule());

            bulider.RegisterType<EFRepositoryContext>().As<IRepositoryContext>().InstancePerLifetimeScope();

            bulider.RegisterType<CMManager>().Named<IModuleManager>(typeof(CMManager).Name);

            // Adapters
            bulider.RegisterType<AutomapperTypeAdapterFactory>().As<ITypeAdapterFactory>().InstancePerLifetimeScope();

            _currentContainer = bulider.Build();
        }
    }
}
