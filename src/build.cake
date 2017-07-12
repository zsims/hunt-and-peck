#tool "nuget:?package=xunit.runner.console"

readonly var target = Argument("target", "Help");
readonly var configuration = Argument("configuration", "Release");

Task("Restore")
    .Does(() =>
{
    NuGetRestore("./HuntAndPeck.sln");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild("./HuntAndPeck.sln", new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        ToolVersion = MSBuildToolVersion.VS2017,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.MSIL
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists("./TestResults");
    XUnit2(new[] {
        string.Format("./HuntAndPeck.Tests/bin/{0}/HuntAndPeck.Tests.dll", configuration)
    }, new XUnit2Settings {
        Parallelism = ParallelismOption.All,
        XmlReport = true,
        ReportName = "results.xml",
        OutputDirectory = "./TestResults"
    });
});

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists("./Dist");
    Zip(string.Format("./HuntAndPeck/bin/{0}", configuration), "./Dist/HuntAndPeck.zip");
});

Task("Default")
    .Does(() => {
    Information(@"
        Targets (run with -Target <target>)
        * Build - Build
        * Test  - Build + run tests
        * Package - Build + package into Dist/
    ");
});

RunTarget(target);