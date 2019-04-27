
var target = Argument("target", "Default");

Task("Build")
    .Does(() => 
    {
        DotNetCoreBuild("Smi.Microsoft.NetCore.Extensions.sln");
    });
    
Task("Default")
    .IsDependentOn("Build");
    
RunTarget(target);