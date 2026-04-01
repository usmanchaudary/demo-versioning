namespace ExcelWorker;

public class ExcelFormatter
{
    public string FormatCellValue(object? value, string format = "General")
    {
        return format switch
        {
            "Currency" => value is decimal or double or float
                ? string.Format("{0:C2}", value)
                : value?.ToString() ?? string.Empty,

            "Percentage" => value is decimal or double or float
                ? string.Format("{0:P2}", value)
                : value?.ToString() ?? string.Empty,

            "Date" => value is DateTime dt
                ? dt.ToString("yyyy-MM-dd")
                : value?.ToString() ?? string.Empty,

            _ => value?.ToString() ?? string.Empty
        };
    }
}
