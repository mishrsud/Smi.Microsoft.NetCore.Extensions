using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Smi.NetCore.Extensions.Hosting;
using Smi.NetCore.Extensions.Hosting.Lifetime;

namespace Sample.SlidingExpirationApp
{
    public class Program
    {
        /*
         * This sample builds and runs a console hosted application that would stop after 10 seconds.
         * In practice, you would have another component in the application that takes a dependency on
         * ILifetimeExpirationCheckpoint and calls ILifetimeExpirationCheckpoint.SetCheckpoint() to notify activity
         * The LifetimeMonitorHostedService will see this activity and will keep the application alive
         *
         * An example is a component that consumes messages off of a queue such as RabbitMQ / SQS / others
         * When a message is received, ILifetimeExpirationCheckpoint.SetCheckpoint() can be called to notify activity.
         * - If another message is received within N seconds (specified using WithSlidingExpirationInterval), the same application
         *   instance will process it.
         * - If no messages arrive in N seconds, the application terminates 
         */
        public static async Task Main(string[] args)
        {
            // NOTE the value provided to WithSlidingExpirationInterval can come from an environment variable 
            await DefaultConsoleHost
                .CreateBuilder(args, "MYAPP_", nameof(SlidingExpirationApp))
                .WithSlidingExpirationInterval(GetValueFromEnvironmentVariableOrDefault)
                .RunConsoleAsync();
        }

        private static int GetValueFromEnvironmentVariableOrDefault()
        {
            if (int.TryParse(Environment.GetEnvironmentVariable("Settings__SlidingExpirationInterval"), out var settingFromEnvironmentVariable))
            {
                return settingFromEnvironmentVariable;
            }
            
            return 30;
        }
    }
}