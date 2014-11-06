using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Linq;
using System.Data.Entity.ModelConfiguration;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base()
        { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public EFDbContext(DbConnection existingConnection)
            : base(existingConnection, true)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            var addMethod = typeof(ConfigurationRegistrar)
            .GetMethods()
            .Single(m =>
                    m.Name == "Add" &&
                    m.GetGenericArguments().Any(a => a.Name == "TEntityType"));

            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name != "EntityFramework");

            foreach (var assembly in assemblies)
            {
                var configTypes = assembly
                    .GetTypes()
                    .Where(t => t.BaseType != null &&
                                t.BaseType.IsGenericType &&
                                t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

                foreach (var type in configTypes)
                {
                    var entityType = type.BaseType.GetGenericArguments().Single();

                    var entityConfig = assembly.CreateInstance(type.FullName);
                    addMethod.MakeGenericMethod(entityType)
                        .Invoke(modelBuilder.Configurations, new[] { entityConfig });
                }
            }
                       
            //modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Configurations.AddFromAssembly(typeof(EFDbContext).Assembly);
            //base.OnModelCreating(modelBuilder);
        }

        
    }
}
