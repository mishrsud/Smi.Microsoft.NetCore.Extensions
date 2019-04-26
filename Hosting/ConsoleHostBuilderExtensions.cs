using Microsoft.Extensions.Hosting;

namespace Smi.NetCore.Extensions.Hosting
{
    public static class ConsoleHostBuilderExtensions
    {
        /// <summary>
        /// Enables a <see cref="IHostBuilder"/> to use a type that implements <see cref="IStartup"/>
        /// for bootstrapping the application. 
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> that is bootstrapping the application</param>
        /// <param name="commandLineArgs">Command line arguments to be passed to the application</param>
        /// <typeparam name="TStartup">A type that implements <see cref="IStartup"/></typeparam>
        /// <returns>
        /// An <see cref="IHostBuilder"/> that has been setup to call IStartup.SetupAppConfiguration
        /// and IStartup.ConfigureServices
        /// </returns>
        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder, 
            string[] commandLineArgs)
            where TStartup : IStartup, new()
        {
            var startup = new TStartup();
            hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                startup.SetupAppConfiguration(
                    hostBuilderContext, 
                    configurationBuilder, 
                    commandLineArgs));

            hostBuilder.ConfigureServices(startup.ConfigureServices);

            return hostBuilder;
        }
    }
}