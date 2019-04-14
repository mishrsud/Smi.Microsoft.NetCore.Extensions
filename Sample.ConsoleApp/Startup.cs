using Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.ConsoleApp;

namespace Sample.Console
{
    public class Startup : IStartup
    {
        /// <summary>
        /// Add Services to DI Container
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="services"></param>
        public void ConfigureServices(
            HostBuilderContext hostBuilderContext, 
            IServiceCollection services)
        {
            
        }

        /// <summary>
        /// Additional configuration
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="configurationBuilder"></param>
        /// <param name="commandLineArgs"></param>
        public void SetupAppConfiguration(
            HostBuilderContext hostBuilderContext, 
            IConfigurationBuilder configurationBuilder,
            string[] commandLineArgs)
        {
                
        }
    }
}