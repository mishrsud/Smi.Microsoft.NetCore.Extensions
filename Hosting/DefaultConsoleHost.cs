using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hosting
{
    public static class DefaultConsoleHost
    {
        public static IHostBuilder CreateBuilder(
            string[] args,
            string environmentVariablePrefix = "ASPNETCORE_",
            string applicationName = null)
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(hostConfiguration =>
                {
                    hostConfiguration.AddEnvironmentVariables(environmentVariablePrefix);
                })
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                {
                    if (args == null) return;
                    configurationBuilder.AddCommandLine(args);
                })
                .UseConsoleLifetime();
        }
        
        public static IHostBuilder CreateBuilder<THostedService>(
            string[] args, 
            string environmentVariablePrefix = "ASPNETCORE_",
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