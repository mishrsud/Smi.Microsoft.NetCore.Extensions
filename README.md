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
1. Add a reference to Smi.NetCore.Extensions.Hosting in your dotnet core console app
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
