using System;

namespace Smi.NetCore.Extensions.Hosting.Lifetime
{
    /// <summary>
    /// Enables the consuming service to set or retrieve Checlpoint
    /// The Checkpoint is a <see cref="DateTimeOffset"/>
    /// </summary>
    public class DefaultLifetimeExpirationCheckpoint : ILifetimeExpirationCheckpoint
    {
        private DateTimeOffset _lastUtcCheckpoint;

        public DefaultLifetimeExpirationCheckpoint()
        {
            SetCheckpoint();
        }
        
        public void SetCheckpoint()
        {
            _lastUtcCheckpoint = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset GetLastUtcCheckpoint()
        {
            return _lastUtcCheckpoint;
        }
    }
}