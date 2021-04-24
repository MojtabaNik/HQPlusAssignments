using Hangfire;
using HQPlusAssignments.Application.Core.Settings;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Application.System;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Extension.DependencyInjection;
using NUnit.Extension.DependencyInjection.Abstractions;
using NUnit.Extension.DependencyInjection.Unity;
using System.Linq;
using Unity;

// tell the extension that we will be using the Microsoft Unity Injection
// factory
[assembly: NUnitTypeInjectionFactory(typeof(UnityInjectionFactory))]

// If we want to manually register the different types we need to create
// one or more implementations of IIocRegistrar that register with the
// container and then use the IocRegistrarTypeDiscoverer.
[assembly: NUnitTypeDiscoverer(typeof(IocRegistrarTypeDiscoverer))]

// The registrar above will scan for implementations of IIocRegistrar,
// which the RegistrarBase class implements, and then execute each
// discovered registrations:

namespace HQPlusAssignments.Application.Test
{
    public class Registrar : RegistrarBase<IUnityContainer>
    {
        protected override void RegisterInternal(IUnityContainer container)
        {
            var interfaces = typeof(IFileService).Assembly.ExportedTypes
                .Where(x => x.IsInterface && x.Name.EndsWith("Service"))
                .ToList();

            var implementations = typeof(FileService).Assembly.ExportedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract && x.Name.EndsWith("Service"))
                .ToList();

            foreach (var @interface in interfaces)
            {
                var implementation = implementations.Find(x => @interface.IsAssignableFrom(x));
                if (implementation == null) continue;
                container.RegisterType(@interface, implementation);
            }

            container.RegisterInstance(typeof(IOptions<MailSettings>), Options.Create<MailSettings>(new MailSettings
            {
                Mail = "c93ab978aca04f",
                DisplayName = "Mojtaba Nikoonejad",
                Host = "smtp.mailtrap.io",
                Port = 587,
                Password = "4e250bb7e1f585"
            }));

            container.RegisterInstance(typeof(IBackgroundJobClient), new Mock<IBackgroundJobClient>().Object);
        }
    }
}
