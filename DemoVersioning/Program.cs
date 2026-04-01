using System.Reflection;
using ExcelWorker;
using ExcelWorker.Settings;
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
        AssemblyVersion = assembly.GetName().Version?.ToString() ?? "unknown";
        FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "unknown";
    }

    public void DisplayVersionInfo()
    {
        AnsiConsole.Write(new FigletText("DemoVersioning").Color(Color.Green));

        // Main App version table
        var appTable = new Table()
            .Title("[bold underline]Main App[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Property[/]")
            .AddColumn("[bold]Value[/]");

        appTable.AddRow("Informational Version", $"[cyan]{InformationalVersion}[/]");
        appTable.AddRow("Assembly Version", $"[yellow]{AssemblyVersion}[/]");
        appTable.AddRow("File Version", $"[yellow]{FileVersion}[/]");
        appTable.AddRow("Version", $"[yellow]{Version}[/]");
        appTable.AddRow("Environment", $"[green]{GetEnvironment()}[/]");
        appTable.AddRow("Runtime", $"[blue]{System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}[/]");

        AnsiConsole.Write(appTable);
        AnsiConsole.WriteLine();

        // ExcelWorker version table
        var workerTable = new Table()
            .Title("[bold underline]ExcelWorker[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Property[/]")
            .AddColumn("[bold]Value[/]");

        workerTable.AddRow("Informational Version", $"[cyan]{ExcelProcessor.Version}[/]");
        workerTable.AddRow("Assembly Version", $"[yellow]{ExcelProcessor.AssemblyVersion}[/]");

        AnsiConsole.Write(workerTable);
        AnsiConsole.WriteLine();

        // ExcelWorker.Settings version table
        var settingsTable = new Table()
            .Title("[bold underline]ExcelWorker.Settings[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Property[/]")
            .AddColumn("[bold]Value[/]");

        settingsTable.AddRow("Informational Version", $"[cyan]{ExcelSettings.Version}[/]");
        settingsTable.AddRow("Assembly Version", $"[yellow]{ExcelSettings.AssemblyVersion}[/]");

        AnsiConsole.Write(settingsTable);

        AnsiConsole.MarkupLine($"\n[grey]ExcelWorker depends on ExcelWorker.Settings — changing Settings bumps both.[/]");
    }

    private static string GetEnvironment()
    {
        return System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Production";
    }
}
