using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Smi.NetCore.Extensions.Hosting
{
    public static class DefaultConsoleHost
    {
        private const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_";
        
        /// <summary>
        /// Creates an <see cref="IHostBuilder"/> that is setup to run as a console application
        /// This host is passed the following environment variables:
        ///     All environment variables starting with ASPNETCORE_
        ///     All environment variables starting with the <paramref name="environmentVariablePrefix"/>,
        ///     if one is passed.
        /// Any command line arguments are also passed through. These can be used to override
        /// environment variables. 
        /// </summary>
        /// <param name="args">Command line arguments to pass to the <see cref="IConfigurationBuilder"/></param>
        /// <param name="environmentVariablePrefix">A custom prefix for the environment variables this application uses.</param>
        /// <param name="applicationName">A name to uniquely identify this app.</param>
        /// <returns>An <see cref="IHostBuilder"/> that uses console lifetime (Keeps running unless indicated to shutdown) </returns>
        public static IHostBuilder CreateBuilder(
            string[] args,
            string environmentVariablePrefix,
            string applicationName = null)
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(hostConfiguration =>
                {
                    hostConfiguration.AddEnvironmentVariables(ASPNETCORE_ENVIRONMENT);
                    if (string.IsNullOrWhiteSpace(environmentVariablePrefix)) return;
                    hostConfiguration.AddEnvironmentVariables(environmentVariablePrefix);
                })
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                {
                    if (!string.IsNullOrWhiteSpace(applicationName))
                    {
                        hostBuilderContext.HostingEnvironment.ApplicationName = applicationName;
                    }
                    
                    if (args == null) return;
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();
                })
                .UseConsoleLifetime();
        }
        
        /// <summary>
        /// Creates an <see cref="IHostBuilder"/> that is setup to run as a console application.
        /// The THostedService is started as part of bootstrapping the application.
        /// This host is passed the following environment variables:
        ///     All environment variables starting with ASPNETCORE_
        ///     All environment variables starting with the <paramref name="environmentVariablePrefix"/>,
        ///     if one is passed.
        /// Any command line arguments are also passed through. These can be used to override
        /// environment variables. 
        /// </summary>
        /// <param name="args">Command line arguments to pass to the <see cref="IConfigurationBuilder"/></param>
        /// <param name="environmentVariablePrefix">A custom prefix for the environment variables this application uses.</param>
        /// <param name="applicationName">A name to uniquely identify this app.</param>
        /// <typeparam name="THostedService">An <see cref="IHostedService"/> implementation to start</typeparam>
        /// <returns></returns>
        public static IHostBuilder CreateBuilder<THostedService>(
            string[] args, 
            string environmentVariablePrefix,
            string applicationName = null) 
                where THostedService : class, IHostedService
        {
            return CreateBuilder(args, environmentVariablePrefix, applicationName)
                .ConfigureServices(
                    (hostBuilderContext, serviceCollection) => 
                        serviceCollection.AddHostedService<THostedService>());
        }
    }
}