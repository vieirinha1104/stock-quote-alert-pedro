namespace stock_quote_alert_pedro;
public static class Utils {
    public static void printWarning(string message) {
        string warning = "Invalid parameters. " + message;
        Console.WriteLine(warning);
        return;
    }
}
