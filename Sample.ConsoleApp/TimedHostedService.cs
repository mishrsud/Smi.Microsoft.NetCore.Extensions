using System;
using System.Threading;
using System.Threading.Tasks;
using Hosting;

namespace Sample.ConsoleApp
{
    public class TimedHostedService : HostedServiceBase, IDisposable
    {
        private Timer _workTimer;
        private const int RunIntervalSeconds = 5;
        
        protected override Task ExecuteLongRunningProcessAsync(CancellationToken cancellationToken)
        {
            _workTimer = new Timer(
                DoWork,
                cancellationToken,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(RunIntervalSeconds));

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            System.Console.WriteLine("Disposing");
            _workTimer?.Change(Timeout.Infinite, 0);
            _workTimer?.Dispose();
        }

        private void DoWork(object state)
        {
            if (state is CancellationToken cancellationToken && cancellationToken.IsCancellationRequested)
            {
                System.Console.WriteLine("Cancelled");
                return;
            }
            
            // Simulate work
            System.Console.WriteLine("Working");
        }

    }
}