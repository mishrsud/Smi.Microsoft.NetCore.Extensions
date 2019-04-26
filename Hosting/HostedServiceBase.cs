using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Smi.NetCore.Extensions.Hosting
{
    /// <summary> A base class to provide correct handling of cancellation and start/stop semantics. </summary>
    /// <remarks>
    /// Using this gist: https://gist.github.com/davidfowl/a7dd5064d9dcf35b6eae1a7953d615e3
    /// from David Fowler of the ASP.NET team
    /// </remarks>
    /// <inheritdoc cref="IHostedService"/>
    public abstract class HostedServiceBase : IHostedService
    {
        private CancellationTokenSource _linkedCancellationTokenSource;
        private Task _executingTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _linkedCancellationTokenSource = 
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteLongRunningProcessAsync(_linkedCancellationTokenSource.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }
            
            _linkedCancellationTokenSource.Cancel();

            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <summary> The long running operation that is run as a hosted service </summary>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to cancel the long running operaion</param>
        /// <returns>A <see cref="Task"/> that represents the operation </returns>
        protected abstract Task ExecuteLongRunningProcessAsync(CancellationToken cancellationToken);
    }
}