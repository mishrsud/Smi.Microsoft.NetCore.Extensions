using System;

namespace Smi.NetCore.Extensions.Hosting.Lifetime
{
    public interface ILifetimeExpirationCheckpoint
    {
        void SetCheckpoint();
        DateTimeOffset GetLastUtcCheckpoint();
    }
}