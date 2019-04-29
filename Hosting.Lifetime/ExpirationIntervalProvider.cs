namespace Smi.NetCore.Extensions.Hosting.Lifetime
{
    public class ExpirationIntervalProvider
    {
        public int IntervalInSeconds { get; }

        public ExpirationIntervalProvider(int intervalInSeconds)
        {
            IntervalInSeconds = intervalInSeconds;
        }
    }
}