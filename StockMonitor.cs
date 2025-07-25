namespace stock_quote_alert_pedro;
public class StockMonitor {
    private volatile bool _buyAlertSent = false;
    private volatile bool _sellAlertSent = false;
    private int _coolDown = 5 * 60;
    public async Task MonitorStockPriceAsync(StockQuoteParameters quote, EmailConfig config, StockQuoteEmail emailClient) {
        var stockPrice = new StockQuotePrice();
        _coolDown = config.CoolDown;
        while (true) {
            double? price = await stockPrice.GetStockPriceAsync(quote.GetSymbol());

            if (!stockPrice.getCheckMarketState) {
                Console.WriteLine("Market is closed. Stopping monitoring.");
                break;
            }

            if (!stockPrice.getCheckTicker) {
                Console.WriteLine("Ticker not found. Stopping monitoring.");
                break;
            }

            if (price == null) {
                Console.WriteLine("Price is null.\n");
                continue;
            }

            if (price > quote.GetUpperBound()) {
                if (!_sellAlertSent) {
                    Console.WriteLine("Price is greater than the upper bound.\n");
                    string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you sell it!";
                    emailClient.SendEmail(config.EmailTo!, "Sell Alert", message);
                    _sellAlertSent = true;
                    _ = Task.Delay(_coolDown * 1000).ContinueWith(_ => _sellAlertSent = false);
                } 
            } else if (price < quote.GetLowerBound()) {
                if (!_buyAlertSent) {
                    Console.WriteLine("Price is lower than the lower bound.\n");
                    string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you buy it!";
                    emailClient.SendEmail(config.EmailTo!, "Buy Alert", message);
                    _buyAlertSent = true;
                    _ = Task.Delay(_coolDown * 1000).ContinueWith(_ => _buyAlertSent = false);
                } 
            }
            await Task.Delay(500);
        }
    }
}
