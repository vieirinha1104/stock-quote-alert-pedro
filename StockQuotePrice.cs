using YahooFinanceApi;

class StockQuotePrice {
    private bool _checkMarketState = true;
    private bool _checkTicker = true;
    public void setCheckTicker(bool ticker) {
        _checkTicker = ticker;
    }
    public void setCheckMarketState(bool marketState) {
        _checkMarketState = marketState;
    }
    public bool getCheckTicker => _checkTicker;
    public bool getCheckMarketState => _checkMarketState;
    public async Task<double?> GetStockPriceAsync(string ticker) {
        try {
            string originalTicker = ticker.ToUpper();
            string formattedTicker = originalTicker + ".SA";

            var securities = await Yahoo.Symbols(formattedTicker)
                .Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketTime)
                .QueryAsync();

            if (!securities.ContainsKey(formattedTicker)) {
                setCheckTicker(false);
                Console.WriteLine($"Ticker '{originalTicker}' not found in the response.\n");
                return null;
            }
            var stock = securities[formattedTicker];

           
            double price = stock[Field.RegularMarketPrice];
            long unixTime = (long)stock[Field.RegularMarketTime];

            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime();

            Console.WriteLine($"[{dateTime:dd/MM/yyyy - HH:mm:ss}] Ticker: {originalTicker} , Price: {price}");
            if (stock[Field.MarketState] == "POSTPOST") {
                setCheckMarketState(false);
                Console.WriteLine($"[WARNING] Market appears to be closed. Last quote was at {dateTime:dd/MM/yyyy - HH:mm:ss}.");
            }

            return price;
        } catch (Exception ex) {
            Console.WriteLine($"Error retrieving stock price: {ex.Message}\n");
            return null;
        }
    }

}
