using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Smi.NetCore.Extensions.Hosting
{
    public interface IStartup
    {
        void ConfigureServices(
            HostBuilderContext hostBuilderContext, 
            IServiceCollection services);

        void SetupAppConfiguration(
            HostBuilderContext hostBuilderContext, 
            IConfigurationBuilder configurationBuilder,
            string[] commandLineArgs);
    }
}