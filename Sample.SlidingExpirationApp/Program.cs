using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Smi.NetCore.Extensions.Hosting;
using Smi.NetCore.Extensions.Hosting.Lifetime;

namespace Sample.SlidingExpirationApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await DefaultConsoleHost
                .CreateBuilder(args, "MYAPP_", nameof(SlidingExpirationApp))
                .WithSlidingExpirationInterval(10)
                .RunConsoleAsync();
        }
    }
}