namespace stock_quote_alert_pedro.Entities;
public class YahooStockQuoteResult {
    public double Price { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string MarketState { get; set; }

    public YahooStockQuoteResult(double price, DateTimeOffset timestamp, string marketstate) {
        Price = price;
        Timestamp = timestamp;
        MarketState = marketstate;
    }
}

