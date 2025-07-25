using YahooFinanceApi;

namespace stock_quote_alert_pedro;
class StockQuotePrice {
    private bool _checkTicker = true;
    public void setCheckTicker(bool ticker) {
        _checkTicker = ticker;
    }
    public bool getCheckTicker => _checkTicker;
    public async Task<YahooStockQuoteResult?> GetYahooStockQuoteAsync(string ticker) {
        try {
            string originalTicker = ticker.ToUpper();
            string formattedTicker = originalTicker + ".SA";

            var securities = await Yahoo.Symbols(formattedTicker)
                .Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketTime, Field.MarketState)
                .QueryAsync();

            if (!securities.ContainsKey(formattedTicker)) {
                setCheckTicker(false);
                Console.WriteLine($"Ticker '{originalTicker}' not found in the response.\n");
                return null;
            }

            var stock = securities[formattedTicker];
            double price = stock[Field.RegularMarketPrice];
            long unixTime = (long)stock[Field.RegularMarketTime];
            string marketState = stock[Field.MarketState];
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime();

            return new YahooStockQuoteResult(price, dateTime, marketState);
        } catch (Exception ex) {
            Console.WriteLine($"Error retrieving stock price: {ex.Message}\n");
            return null;
        }
    }

}
