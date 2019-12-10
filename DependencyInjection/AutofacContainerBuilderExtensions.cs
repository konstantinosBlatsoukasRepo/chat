using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Hosting;
using System;

namespace Chat.DependencyInjection
{
    public static class AutofacContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the <see cref="IContainer"/> to inject itself.
        /// </summary>
        /// <returns>The original container builder.</returns>
        public static ContainerBuilder AddAutofac(this ContainerBuilder containerBuilder)
        {
            IContainer container = null;

            containerBuilder.Register(c => container)
                .AsSelf()
                .As<ILifetimeScope>();

            containerBuilder.RegisterBuildCallback(c => container = c);

            return containerBuilder;
        }

        /// <summary>
        /// Registers the type <typeparamref name="THostedService"/> as a <see cref="IHostedService"/> to start when application starts.
        /// </summary>
        /// <typeparam name="THostedService">The type to register.</typeparam>
        /// <param name="containerBuilder">The container builder to register the hosted service.</param>
        /// <returns>The original container builder.</returns>
        public static IRegistrationBuilder<THostedService, ConcreteReflectionActivatorData, SingleRegistrationStyle> AddHostedService<THostedService>(this ContainerBuilder containerBuilder)
            where THostedService : class, IHostedService, IDisposable
        {
            return containerBuilder.RegisterType<THostedService>()
                .AsSelf()
                .As<IHostedService>();
        }
    }
}
