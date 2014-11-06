using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Reflection;

namespace ChinaUnicom.Fuyang.Framework.Adapter
{
    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {
        public AutomapperTypeAdapterFactory()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var profiles = new List<Type>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var typesInAssembly = assembly.GetTypes().Where(p => p.BaseType == typeof(Profile));

                    profiles.AddRange(typesInAssembly);
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }

            Mapper.Initialize(cfg => 
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.SelfProfiler`2")
                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);                            
                }
            });
        }

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }
    }
}
