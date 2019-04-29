using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Smi.NetCore.Extensions.Hosting.Lifetime
{
    public class LifetimeMonitorHostedService : HostedServiceBase, IDisposable
    {
        private readonly ILifetimeExpirationCheckpoint _lifetimeExpirationCheckpoint;
        private readonly IHost _underlyingHost;
        private readonly ILogger<LifetimeMonitorHostedService> _logger;
        private Timer _workTimer;
        private TimeSpan _keepAliveThresholdSeconds;

        public int MonitorIntervalSeconds { get; internal set; }

        public LifetimeMonitorHostedService(
            ILifetimeExpirationCheckpoint lifetimeExpirationCheckpoint,
            IHost underlyingHost,
            ExpirationIntervalProvider expirationIntervalProvider,
            ILogger<LifetimeMonitorHostedService> logger)
        {
            _lifetimeExpirationCheckpoint = lifetimeExpirationCheckpoint;
            _underlyingHost = underlyingHost;
            MonitorIntervalSeconds = expirationIntervalProvider.IntervalInSeconds;
            _logger = logger;
        }
        
        protected override Task ExecuteLongRunningProcessAsync(CancellationToken cancellationToken)
        {
            var keepAliveCheckInterval = GetValueOrDefault(MonitorIntervalSeconds);
            _keepAliveThresholdSeconds = TimeSpan.FromSeconds(keepAliveCheckInterval);
            _logger.LogInformation("Configuring Host Lifetime monitoring to check every {CheckPeriod} seconds", keepAliveCheckInterval);
            
            _workTimer = new Timer(
                DoKeepAliveCheck,
                cancellationToken,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(keepAliveCheckInterval));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("{ServiceName} Stopping", nameof(LifetimeMonitorHostedService));
            _workTimer?.Change(Timeout.Infinite, 0);
            _workTimer?.Dispose();
        }
        
        private void DoKeepAliveCheck(object state)
        {
            var lastUtcCheckpoint = _lifetimeExpirationCheckpoint.GetLastUtcCheckpoint();
            var curentUtcTime = DateTimeOffset.UtcNow;

            if (curentUtcTime - lastUtcCheckpoint > _keepAliveThresholdSeconds)
            {
                _logger.LogWarning("{ServiceName} detected breach of KeepAlive threshold, Initiating stop", nameof(LifetimeMonitorHostedService));
                _underlyingHost.StopAsync(TimeSpan.Zero).GetAwaiter().GetResult();
                return;
            }
            
            _logger.LogDebug("{ServiceName} keep alive check passed");
        }

        private int GetValueOrDefault(int monitorIntervalSeconds)
        {
            return monitorIntervalSeconds == 0 ? 60 : monitorIntervalSeconds;
        }
    }
}