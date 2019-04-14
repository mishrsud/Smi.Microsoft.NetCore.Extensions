using Microsoft.Extensions.Hosting;

namespace Hosting
{
    public static class ConsoleHostBuilderExtensions
    {
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