using System.Reflection;

namespace ExcelWorker;

public class ExcelProcessor
{
    public static string Version =>
        Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "unknown";

    public static string AssemblyVersion =>
        Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "unknown";

    public void ProcessWorkbook(string filePath)
    {
        // Simulated Excel processing
        Console.WriteLine($"[ExcelWorker v{Version}] Processing: {filePath}");
    }
}
