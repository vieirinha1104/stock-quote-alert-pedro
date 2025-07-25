using stock_quote_alert_pedro;
using YahooFinanceApi;

public class StockMonitor {
    private int _coolDown = 5 * 60;
    private long _lastTimestamp = 0;
    private long _lastSellAlertTimestamp = 0;
    private long _lastBuyAlertTimestamp = 0;
    private bool _marketClosed = false;
    private bool _marketPreOpening = false;
    private bool checkTimestamp(long currentTimestamp) {
        if (_lastTimestamp == 0) {
            _lastTimestamp = currentTimestamp;
            return false;
        }
        if (_lastTimestamp == currentTimestamp) {
            return true;
        }
        _lastTimestamp = currentTimestamp;
        return false;
    }

    public async Task MonitorStockPriceAsync(StockQuoteParameters quote, Config config, StockQuoteEmail emailClient) {
        var stockPrice = new StockQuotePrice();
        _coolDown = config.CoolDown;

        while (true) {
            var result = await stockPrice.GetYahooStockQuoteAsync(quote.GetSymbol());

            if (result == null) {
                Console.WriteLine("Price result is null.\n");
                continue;
            }

            long currentTimestamp = result.Timestamp.ToUnixTimeSeconds();
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(currentTimestamp).ToLocalTime();
            double price = result.Price;
            string marketState = result.MarketState;
            if (marketState == "POSTPOST" && !_marketClosed) {
                _marketClosed = true;
                Console.WriteLine($"[{dateTime:dd/MM/yyyy - HH:mm:ss}] [WARNING] Market appears to be closed.\n");
                if (config.StopProgramIfMarketIsClosed) break;
            } 
            if (true && !_marketPreOpening) {
                _marketPreOpening = true;
                Console.WriteLine($"[{dateTime:dd/MM/yyyy - HH:mm:ss}] [WARNING] Market appears to be in pre-opening.\n");
            }
            if (checkTimestamp(currentTimestamp)) {
                 await Task.Delay(500);
                continue;
            }
            
            Console.WriteLine($"[{dateTime:dd/MM/yyyy - HH:mm:ss}] Ticker: {quote.GetSymbol().ToUpper()} , Price: {price}\n");
            if (price > quote.GetUpperBound()) {
                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you sell it!";
                if (now - _lastSellAlertTimestamp >= _coolDown) {
                    Console.WriteLine("Price is greater than the upper bound.\n");
                    emailClient.SendEmail(config.EmailTo!, "Sell Alert", message);
                    _lastSellAlertTimestamp = now;
                }
            }
            if (price < quote.GetLowerBound()) {
                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                string message = $"Hey, The Price for {quote.GetSymbol()} is {price}. I'd recommend you buy it!";
                if (now - _lastBuyAlertTimestamp >= _coolDown) {
                    Console.WriteLine("Price is lower than the lower bound.\n");
                    emailClient.SendEmail(config.EmailTo!, "Buy Alert", message);
                    _lastBuyAlertTimestamp = now;
                }
            }

            await Task.Delay(500);
        }
    }
}
