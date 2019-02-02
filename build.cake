// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var distDirectory = Directory("./dist");

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(distDirectory);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore("./src/Runpath.sln");
    });

// Build using the build configuration specified as an argument.
 Task("Build")
    .Does(() =>
    {
        DotNetCoreBuild("./src/Runpath.sln",
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("UnitTests")
  .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./**/*.Unit.Tests.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true
                });
        }
    });
	
Task("IntegrationTests")
  .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("./**/*.Integration.Tests.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true
                });
        }
    });

// Publish the app to the /dist folder
Task("PublishWeb")
    .Does(() =>
    {
        DotNetCorePublish(
            "./src/Runpath.Api/Runpath.Api.csproj",
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = distDirectory,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });
	
// Publish the app to the /dist folder
Task("Run")
    .IsDependentOn("BuildAndTest")
    .IsDependentOn("PublishWeb")
    .Does(() =>
    {
        var settings = new DotNetCoreExecuteSettings
        {
            WorkingDirectory = distDirectory
        };

        DotNetCoreExecute(distDirectory.ToString() + "/Runpath.Api.dll", "", settings);
    });

// A meta-task that runs all the steps to Build and Test the app
Task("BuildAndTest")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests")
    .IsDependentOn("IntegrationTests");

// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Publish.
Task("Default")
    .IsDependentOn("BuildAndTest")
    .IsDependentOn("PublishWeb");

// Executes the task specified in the target argument.
RunTarget(target);