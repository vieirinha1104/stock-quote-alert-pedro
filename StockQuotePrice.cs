using YahooFinanceApi;

class StockQuotePrice {
    public static async Task<double?> GetStockPriceAsync(string ticker) {
        try {
            string aux_ticker = ticker;
            ticker.ToUpper();
            ticker += ".SA";
            var securities = await Yahoo.Symbols(ticker).Fields(Field.Symbol, Field.RegularMarketPrice).QueryAsync();
            if (!securities.ContainsKey(ticker)) {
                throw new KeyNotFoundException($"Ticker '{aux_ticker}' not found in the response.");
            }
            var stock = securities[ticker];
            Console.WriteLine($"Current stock price is: {stock[Field.RegularMarketPrice]}\n");
            return stock[Field.RegularMarketPrice];
        } catch (Exception ex) {
            Console.WriteLine($"Error retrieving stock price: {ex.Message}");
            return null;
        }
    }
}
