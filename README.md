[![Build status](https://ci.appveyor.com/api/projects/status/bpp90v9fgwsc1nof?svg=true)](https://ci.appveyor.com/project/mishrsud/smi-microsoft-netcore-extensions)


# Smi.Microsoft.NetCore.Extensions
A collection of useful extensions for .NET Core applications

## 1. Smi.NetCore.Extensions.Hosting
### Purpose
Whilst working with an ASP.NET Core app, we have the hosting infrastructure to leverage. IWebHostBuidler exposes this
and the default project template creates a Program.cs that calls into Startup.cs to bootstrap the application.
For non ASP.NET application, we have IHostBuilder and IHost to use. 

By default, using the Microsoft.Extensions.Hosting.Abstractions and Microsoft.Extensions.Hosting packages creates a Program.cs that has more than once concern thrown in:
- Create the Host
- Read and Wire up configuration from various sources
- Map custom services into the dependency injection container.

This project aims to provide infrastructure that lets one create a console hosted application similar to an ASP.NET application with cleanly separated Program and Startup such that:
- Program.cs has the sole responsibility of creating the host and specifying the Startup class
- Startup.cs has the responsibility of configuring the application processing pipeline and dependency injection.  
  
### How to use
1. Add a reference to **Smi.NetCore.Extensions.Hosting** in your dotnet core console app
```bash
# Package Manager console
Install-Package Smi.NetCore.Extensions.Hosting 
# dotnet CLI
dotnet add package Smi.NetCore.Extensions.Hosting
```
2. Initialize your host like so:

```csharp
public class Program
    {
        public static async Task Main(string[] args)
        {
            // NOTE: TimedHostedService is an IHostedService implementation
            // Startup is a class that implements the IStartup interface
            await DefaultConsoleHost
                .CreateBuilder<TimedHostedService>(   
                    args, 
                    "MYAPP_",
                    nameof(Sample.ConsoleApp))
                .UseStartup<Startup>(args)
                .RunConsoleAsync();
        }
    }
```

Your application can now run as a daemon/service/dockerised app.
An example app is create in the form of Sample.TimedConsoleApp.

## 2. Smi.NetCore.Extensions.Hosting.Lifetime

### Purpose
Scenario:
- Your app runs in a serverless environment where you are charged for how long the app is running. Hence, you design the app to do its job and then terminate. 
- During busy periods, you want to avoid the cost of boostrapping the app 
- You want your app to have some way of knowing how to keep itself up based on a threshold that is dynamically computed based on an activity

### How to use
1. Add a reference to Smi.NetCore.Extensions.Hosting.Lifetime

```bash
# Package Manager console
Install-Package Smi.NetCore.Extensions.Hosting.Lifetime
# dotnet CLI
dotnet add package Smi.NetCore.Extensions.Hosting.Lifetime
```

2. Next, construct a Host like so:

```csharp
public class Program
    {
        public static async Task Main(string[] args)
        {
            // NOTE the value provided to WithSlidingExpirationInterval can come from an environment variable 
            await DefaultConsoleHost
                .CreateBuilder(args, "MYAPP_", nameof(SlidingExpirationApp))
                .WithSlidingExpirationInterval(10)
                .UseStartup<Startup>(args)
                .RunConsoleAsync();
        }
    }
```

Notice the ```WithSlidingExpirationInterval(10)``` call. This call:
1. Starts a HostedService that monitors application activity by means of the ```ILifetimeExpirationCheckpoint``` interface. This interface is registered in the IServiceCollection as a singleton.
2. The application signals activity by calling ILifetimeExpirationCheckpoint.SetCheckpoint(). In the above example, if the application called SetCheckpoint within 10 seconds of starting up, it will continue. Otherwise, it will shutdown gracefully. The code to signal activity may look like so:

If you need to read the Interval from a configuration source (such as Environment variable / appsettings.json etc) you can use this overload of WithSlidingExpirationInterval:
```csharp
public class Program
    {
        public static async Task Main(string[] args)
        {
            // NOTE the value provided to WithSlidingExpirationInterval can come from an environment variable 
            await DefaultConsoleHost
                .CreateBuilder(args, "MYAPP_", nameof(SlidingExpirationApp))
                .WithSlidingExpirationInterval(() => 
                {
                    // read value from config here
                    int interval = ReadFromConfig()
                    return interval;
                })
                .UseStartup<Startup>(args)
                .RunConsoleAsync();
        }
    }
```

Example of using ILifetimeExpirationCheckpoint to signal activity 

```csharp
public class MessageProcessor
{
    private readonly ILifetimeExpirationCheckpoint _lifetimeExpirationCheckpoint;

    public MessageProcessor(ILifetimeExpirationCheckpoint lifetimeExpirationCheckpoint)
    {
        _lifetimeExpirationCheckpoint = lifetimeExpirationCheckpoint;
    }

    public Task ProcessMessage(Message message)
    {
        //do something with the message

        _lifetimeExpirationCheckpoint.SetCheckpoint();   
    }
}
```

An example is created in the form of Sample.SlidingExpirationHost