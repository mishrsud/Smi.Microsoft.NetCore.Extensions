using System;
using System.Threading;
using System.Threading.Tasks;
using Sample.TimedConsoleApp.Services;
using Smi.NetCore.Extensions.Hosting;

namespace Sample.TimedConsoleApp
{
    public class TimedHostedService : HostedServiceBase, IDisposable
    {
        private readonly IOutputWriter _outputWriter;
        private Timer _workTimer;
        private const int RunIntervalSeconds = 5;

        public TimedHostedService(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }
        
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
            _outputWriter.Write($"{nameof(TimedHostedService)} is being disposed");
            _workTimer?.Change(Timeout.Infinite, 0);
            _workTimer?.Dispose();
        }

        private void DoWork(object state)
        {
            if (state is CancellationToken cancellationToken && cancellationToken.IsCancellationRequested)
            {
                _outputWriter.Write($"{nameof(TimedHostedService)} has received Cancellation");
                return;
            }
            
            // Simulate work
            _outputWriter.Write($"{nameof(TimedHostedService)} is working");
        }

    }
}