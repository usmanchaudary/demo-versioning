using System.Reflection;

namespace DemoVersioning;

/// <summary>
/// Example showing how to expose version info via a minimal API endpoint.
/// To use this, switch the project to a web app or reference it from one.
///
/// Usage in a web API:
///   app.MapVersionEndpoint();
/// </summary>
public static class VersionEndpoint
{
    public static object GetVersionInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return new
        {
            version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
            assemblyVersion = assembly.GetName().Version?.ToString(),
            fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
            environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production",
            timestamp = DateTime.UtcNow
        };
    }
}
