# Smi.Microsoft.NetCore.Extensions
A collection of useful extensions for .NET Core applications

# How to use
1. Add a reference to Smi.Microsoft.NetCore.Extensions.Hosting in your dotnet core console app
2. Initialize your host like so:

```csharp
public class Program
    {
        public static async Task Main(string[] args)
        {
            await DefaultConsoleHost
                .CreateBuilder<TimedHostedService>(   // TimedHostedService is an IHostedService implementation
                    args, 
                    "MYAPP_",
                    nameof(Sample.ConsoleApp))
                .RunConsoleAsync();
        }
    }
```

Your application can now run as a daemon/service/dockerised app.
