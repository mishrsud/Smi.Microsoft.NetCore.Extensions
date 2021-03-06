using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.TimedConsoleApp.Services;
using Smi.NetCore.Extensions.Hosting;

namespace Sample.TimedConsoleApp
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
            Configuration = hostBuilderContext.Configuration;
            services.AddScoped<IOutputWriter, ConsoleOutputWriter>();
        }

        public IConfiguration Configuration { get; private set; }

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
            // Additional configuration     
        }
    }
}