using System.Reflection;

namespace stock_quote_alert_pedro;
public static class Utils {
    public static void printWarning(string message) {
        string warning = "Invalid parameters. " + message;
        Console.WriteLine(warning);
        return;
    }
    public static bool HasInvalidConfig(Config config) {
        var properties = typeof(Config).GetProperties();

        foreach (var prop in properties) {
            var value = prop.GetValue(config);

            if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(value as string))
                return true;

            if (prop.PropertyType == typeof(int) && (int)value == 0)
                return true;

            if (prop.PropertyType == typeof(bool) && value == null)
                return true;
        }

        return false;
    }
}
