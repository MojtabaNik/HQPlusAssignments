using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace HQPlusAssignments.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonsByConvention(
    this IServiceCollection services,
       Assembly interfaceAssembly,
       Assembly implementationAssembly,
    Func<Type, bool> predicate) => services.AddSingletonsByConvention(interfaceAssembly, implementationAssembly, predicate, predicate);

        public static IServiceCollection AddSingletonsByConvention(
            this IServiceCollection services,
            Assembly interfaceAssembly,
            Assembly implementationAssembly,
            Func<Type, bool> interfacePredicate,
            Func<Type, bool> implementationPredicate)
        {
            var interfaces = interfaceAssembly.ExportedTypes
                .Where(x => x.IsInterface && interfacePredicate(x))
                .ToList();

            var implementations = implementationAssembly.ExportedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract && implementationPredicate(x))
                .ToList();

            foreach (var @interface in interfaces)
            {
                var implementation = implementations.Find(x => @interface.IsAssignableFrom(x));
                if (implementation == null) continue;
                services.AddSingleton(@interface, implementation);
            }
            return services;
        }
    }
}
