using System.Reflection;

namespace ExcelWorker.Settings;

public class ExcelSettings
{
    public string DefaultDateFormat { get; set; } = "yyyy-MM-dd";
    public string DefaultCurrencySymbol { get; set; } = "$";
    public int MaxRowsPerSheet { get; set; } = 1_048_576;
    public int MaxColumnsPerSheet { get; set; } = 16_384;
    public bool AutoTrimCellValues { get; set; } = true;
    public bool IgnoreEmptyRows { get; set; } = true;

    public static string Version =>
        Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "unknown";

    public static string AssemblyVersion =>
        Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "unknown";
}
