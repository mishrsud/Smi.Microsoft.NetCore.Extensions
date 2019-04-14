using System.Threading.Tasks;
using Hosting;
using Microsoft.Extensions.Hosting;

namespace Sample.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await DefaultConsoleHost
                .CreateBuilder<TimedHostedService>(
                    args, 
                    "MYAPP_",
                    nameof(Sample.ConsoleApp))
                .RunConsoleAsync();
        }
    }
}