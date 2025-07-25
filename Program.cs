using System.Globalization;

namespace stock_quote_alert_pedro;
class Program {
    static async Task Main(string[] args) {
        if (!TryParseArguments(args, out StockQuoteParameters? quote))
            return;

        Config config = ConfigLoader.LoadEmailConfig("JsonConfigFile.json");

        if (Utils.HasInvalidConfig(config)) {
            Console.WriteLine("Error: Invalid configuration in the JSON file.\n");
            return;
        }
        StockQuoteEmail emailClient = new StockQuoteEmail(config);

        Console.WriteLine($"Monitoring {quote.GetSymbol().ToUpper()}:");
        Console.WriteLine($"Alert if price < {quote.GetLowerBound()} or > {quote.GetUpperBound()}\n");

        StockMonitor monitor = new StockMonitor();
        await monitor.MonitorStockPriceAsync(quote, config, emailClient);
    }

    static bool TryParseArguments(string[] args, out StockQuoteParameters? quote) {
        quote = null;
        if (args.Length != 3) {
            Utils.printWarning("Usage: <program> <SYMBOL> <UPPER_BOUND> <LOWER_BOUND>\n");
            return false;
        }

        string symbol = args[0];

        if (!double.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double upper)) {
            Utils.printWarning("Invalid upper bound.\n");
            return false;
        }

        if (!double.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double lower)) {
            Utils.printWarning("Invalid lower bound.\n");
            return false;
        }

        if (upper < lower) {
            Utils.printWarning("The upper bound must not be lower than the lower bound.\n");
            return false;
        }

        quote = new StockQuoteParameters();
        quote.SetSymbol(symbol);
        quote.SetUpperBound(upper);
        quote.SetLowerBound(lower);
        return true;
    }
}
