using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Sample.Console;
using Smi.NetCore.Extensions.Hosting;

namespace Sample.TimedConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await DefaultConsoleHost
                .CreateBuilder<TimedHostedService>(
                    args, 
                    "MYAPP_",
                    nameof(TimedConsoleApp))
                .UseStartup<Startup>(args)
                .RunConsoleAsync();
        }
    }
}