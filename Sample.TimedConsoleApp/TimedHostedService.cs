using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sample.TimedConsoleApp.Services;
using Smi.NetCore.Extensions.Hosting;

namespace Sample.TimedConsoleApp
{
    public class TimedHostedService : HostedServiceBase, IDisposable
    {
        private readonly IOutputWriter _outputWriter;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _workTimer;
        private const int RunIntervalSeconds = 5;

        public TimedHostedService(IOutputWriter outputWriter, ILogger<TimedHostedService> logger)
        {
            _outputWriter = outputWriter;
            _logger = logger;
        }
        
        protected override Task ExecuteLongRunningProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            _workTimer = new Timer(
                DoWork,
                cancellationToken,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(RunIntervalSeconds));

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            _logger.LogWarning("Disposing");
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
            _logger.LogInformation("Service running");
            _outputWriter.Write($"{nameof(TimedHostedService)} is working");
        }

    }
}