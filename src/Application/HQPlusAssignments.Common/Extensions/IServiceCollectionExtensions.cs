using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace HQPlusAssignments.Common.Extensions
{
    /// <summary>
    /// This class is used for dependecy injection.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// An overrided method for cases that interface and service condition are the same
        /// </summary>
        /// <param name="services"></param>
        /// <param name="interfaceAssembly"></param>
        /// <param name="implementationAssembly"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonsByConvention(
    this IServiceCollection services,
       Assembly interfaceAssembly,
       Assembly implementationAssembly,
    Func<Type, bool> predicate) => services.AddSingletonsByConvention(interfaceAssembly, implementationAssembly, predicate, predicate);

        /// <summary>
        /// Instead of registering All services one by one, we can use reflection to register all by name convention
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="interfaceAssembly">The assembly in which interfaces are</param>
        /// <param name="implementationAssembly">The assembly in which implementations are</param>
        /// <param name="interfacePredicate">The condition for name of interfaces</param>
        /// <param name="implementationPredicate">The condition for name of services</param>
        /// <returns>Service container which all services registered</returns>
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
