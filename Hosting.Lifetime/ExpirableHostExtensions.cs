using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Smi.NetCore.Extensions.Hosting.Lifetime
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Extension method")]
    public static class ExpirableHostExtensions
    {
        /// <summary>
        /// Adds a hosted service that monitors activity on the application running this host.
        /// The host is stopped if there is no activity in the specified interval.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> being used to build the <see cref="IHost"/></param>
        /// <param name="slidingExpirationIntervalSeconds">A value that specifies number of seconds within which an activity will keep the host alive</param>
        /// <returns></returns>
        public static IHostBuilder WithSlidingExpirationInterval
            (
            this IHostBuilder hostBuilder,
            int slidingExpirationIntervalSeconds)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddSingleton<ILifetimeExpirationCheckpoint, DefaultLifetimeExpirationCheckpoint>();
                    services.AddSingleton(provider => new ExpirationIntervalProvider(slidingExpirationIntervalSeconds));
                    services.AddHostedService<LifetimeMonitorHostedService>();
                });
            return hostBuilder;
        }
        
        /// <summary>
        /// Adds a hosted service that monitors activity on the application running this host.
        /// The host is stopped if there is no activity in the specified interval.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> being used to build the <see cref="IHost"/></param>
        /// <param name="getIntervalDelegate">
        /// A delegate that returns an integer number of seconds within which an activity will keep the host alive
        /// </param>
        /// <returns></returns>
        public static IHostBuilder WithSlidingExpirationInterval
        (
            this IHostBuilder hostBuilder,
            Func<int> getIntervalDelegate)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, services) =>
            {
                int slidingExpirationIntervalSeconds = getIntervalDelegate();
                services.AddSingleton<ILifetimeExpirationCheckpoint, DefaultLifetimeExpirationCheckpoint>();
                services.AddSingleton(provider => new ExpirationIntervalProvider(slidingExpirationIntervalSeconds));
                services.AddHostedService<LifetimeMonitorHostedService>();
            });
            return hostBuilder;
        }
    }
}