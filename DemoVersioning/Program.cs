using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<VersionService>();
builder.Services.AddHostedService<App>();

using var host = builder.Build();
await host.RunAsync();

public class App : BackgroundService
{
    private readonly VersionService _versionService;
    private readonly IHostApplicationLifetime _lifetime;

    public App(VersionService versionService, IHostApplicationLifetime lifetime)
    {
        _versionService = versionService;
        _lifetime = lifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _versionService.DisplayVersionInfo();
        _lifetime.StopApplication();
        return Task.CompletedTask;
    }
}

public class VersionService
{
    public string Version { get; }
    public string InformationalVersion { get; }
    public string AssemblyVersion { get; }
    public string FileVersion { get; }

    public VersionService()
    {
        var assembly = Assembly.GetExecutingAssembly();
        Version = assembly.GetName().Version?.ToString() ?? "unknown";
        InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
        AssemblyVersion = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "unknown";
        FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";
    }

    public void DisplayVersionInfo()
    {
        AnsiConsole.Write(new FigletText("DemoVersioning").Color(Color.Green));

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Property[/]")
            .AddColumn("[bold]Value[/]");

        table.AddRow("Informational Version", $"[cyan]{InformationalVersion}[/]");
        table.AddRow("Assembly Version", $"[yellow]{AssemblyVersion}[/]");
        table.AddRow("File Version", $"[yellow]{FileVersion}[/]");
        table.AddRow("Version", $"[yellow]{Version}[/]");
        table.AddRow("Environment", $"[green]{GetEnvironment()}[/]");
        table.AddRow("Runtime", $"[blue]{System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}[/]");
        table.AddRow("OS", $"[blue]{System.Runtime.InteropServices.RuntimeInformation.OSDescription}[/]");

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"\n[grey]Built from git history using GitVersion + Semantic Versioning[/]");
    }

    private static string GetEnvironment()
    {
        return System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Production";
    }
}
