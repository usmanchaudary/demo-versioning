namespace ExcelWorker;

public class ExcelReader
{
    public IEnumerable<Dictionary<string, string>> ReadRows(string filePath, bool hasHeader = true)
    {
        // Simulated Excel reading — in production, use a library like EPPlus or ClosedXML
        Console.WriteLine($"[ExcelReader v{ExcelProcessor.Version}] Reading: {filePath} (hasHeader={hasHeader})");

        // Return sample data for demo purposes
        yield return new Dictionary<string, string>
        {
            ["Name"] = "Sample Row",
            ["Value"] = "Demo Data"
        };
    }

    public int GetRowCount(string filePath)
    {
        // Simulated row count
        Console.WriteLine($"[ExcelReader v{ExcelProcessor.Version}] Counting rows in: {filePath}");
        return 0;
    }
}
