using System.Globalization;

namespace stock_quote_alert_pedro;
class Program {
    static async Task Main(string[] args) {
        if (!TryParseArguments(args, out StockQuoteParameters? quote))
            return;

        EmailConfig config = ConfigLoader.LoadEmailConfig("JsonConfigFile.json");

        if (string.IsNullOrWhiteSpace(config.SmtpHost) || string.IsNullOrWhiteSpace(config.HostEmail) || string.IsNullOrWhiteSpace(config.HostPassword) || string.IsNullOrWhiteSpace(config.EmailTo)) {
            Console.WriteLine("Erro: configuração de e-mail inválida no arquivo JSON.");
            return;
        }
        StockQuoteEmail emailClient = new StockQuoteEmail(config);

        Console.WriteLine($"Monitoring {quote.GetSymbol()}...");
        Console.WriteLine($"Alert if price < {quote.GetLowerBound()} or > {quote.GetUpperBound()}\n");

        await MonitorStockPriceAsync(quote, config, emailClient);
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
        
        if (upper <= lower) {
            Utils.printWarning("Upper bound must be greater than lower bound.\n");
            return false;
        }

        quote = new StockQuoteParameters();
        quote.SetSymbol(symbol);
        quote.SetUpperBound(upper);
        quote.SetLowerBound(lower);
        return true;
    }

    static async Task MonitorStockPriceAsync(StockQuoteParameters quote, EmailConfig config, StockQuoteEmail emailClient) {
        bool buyAlertSent = false;
        bool sellAlertSent = false;
        int coolDown = 5*60; 
        while (true) {
            double? price = await StockQuotePrice.GetStockPriceAsync(quote.GetSymbol());

            if (price == null) {
                Console.WriteLine("Price is null.\n");
                continue;
            }

            if (price > quote.GetUpperBound()) {
                Console.WriteLine("Price is greater than the upper bound.\n");
                if (!sellAlertSent) {
                    string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you sell it!";
                    emailClient.SendEmail(config.EmailTo!, "Sell Alert", message);
                    sellAlertSent = true;

                    _ = Task.Delay(coolDown * 1000).ContinueWith(_ => sellAlertSent = false);
                } else {
                    Console.WriteLine("Sell alert already sent. Waiting for cooldown.\n");
                }
            } else if (price < quote.GetLowerBound()) {
                Console.WriteLine("Price is lower than the lower bound.\n");
                if (!buyAlertSent) {
                    string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you buy it!";
                    emailClient.SendEmail(config.EmailTo!, "Buy Alert", message);
                    buyAlertSent = true;

                    _ = Task.Delay(coolDown * 1000).ContinueWith(_ => buyAlertSent = false);
                } else {
                    Console.WriteLine("Buy alert already sent. Waiting for cooldown.\n");
                }
            }
            await Task.Delay(500);
        }
    }
}
