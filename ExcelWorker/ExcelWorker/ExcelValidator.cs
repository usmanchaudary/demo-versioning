namespace ExcelWorker;

public class ExcelValidator
{
    public bool ValidateHeaders(string[] expectedHeaders, string[] actualHeaders)
    {
        if (expectedHeaders.Length != actualHeaders.Length)
            return false;

        for (int i = 0; i < expectedHeaders.Length; i++)
        {
            if (!string.Equals(expectedHeaders[i], actualHeaders[i], StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }
}
