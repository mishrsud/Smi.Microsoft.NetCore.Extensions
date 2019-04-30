#module nuget:?package=Cake.DotNetTool.Module&version=0.2.0
#tool dotnet:?package=GitVersion.Tool&version=5.0.0-beta2-75

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

var artifactsDir = MakeAbsolute(Directory("artifacts"));
var packagesDir = artifactsDir.Combine(Directory("packages"));
var solutionPath = "./Smi.NetCore.Extensions.sln";

Task("Clean")
    .Does(() => 
    {
        CleanDirectory(artifactsDir);

        var settings = new DotNetCoreCleanSettings
        {
            Configuration = configuration
        };

        DotNetCoreClean(solutionPath, settings);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
    {
        DotNetCoreRestore();
    });

Task("SemanticVersion")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var gitVersionSettings = new GitVersionSettings
        {
            NoFetch = true,
        };

        var gitVersion = GitVersion(gitVersionSettings);

        assemblyVersion = gitVersion.AssemblySemVer;
        packageVersion = gitVersion.NuGetVersion;

        Information($"AssemblySemVer: {assemblyVersion}");
        Information($"NuGetVersion: {packageVersion}");
    });

Task("SetAppVeyorVersion")
    .IsDependentOn("SemanticVersion")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
       // AppVeyor.UpdateBuildVersion(packageVersion);
    });

Task("Build")
    .IsDependentOn("SetAppVeyorVersion")
    .Does(() => 
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoIncremental = true,
            NoRestore = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .SetVersion(assemblyVersion)
                .WithProperty("FileVersion", packageVersion)
                .WithProperty("InformationalVersion", packageVersion)
                .WithProperty("nowarn", "7035")
        };

        DotNetCoreBuild(solutionPath, settings);
    });
    
Task("Pack")
    .IsDependentOn("Build")
    .Does(() => 
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = "Release",
            NoBuild = true,
            NoRestore = true,
            OutputDirectory = packagesDir,
            IncludeSource = true,
            IncludeSymbols = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .WithProperty("PackageVersion", packageVersion)
                .WithProperty("Copyright", $"Copyright Sudhanshu Mishra {DateTime.Now.Year}")
        };
        DotNetCorePack("./Hosting/Hosting.csproj", settings);
        DotNetCorePack("./Hosting.Lifetime/Hosting.Lifetime.csproj", settings);
    });

Task("PublishAppVeyorArtifacts")
    .IsDependentOn("Pack")
    .WithCriteria(() => HasArgument("pack") && AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        CopyFiles($"{packagesDir}/*.nupkg", MakeAbsolute(Directory("./")), false);

        GetFiles($"./*.nupkg")
            .ToList()
            .ForEach(f => AppVeyor.UploadArtifact(f, new AppVeyorUploadArtifactsSettings { DeploymentName = "packages" }));
    });

Task("Default")
    .IsDependentOn("PublishAppVeyorArtifacts");
    
RunTarget(target);