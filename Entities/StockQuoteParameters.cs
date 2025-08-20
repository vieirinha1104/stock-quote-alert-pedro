namespace stock_quote_alert_pedro.Entities;
public class StockQuoteParameters {
    private string _symbol = "";
    private double _upperBound = 0;
    private double _lowerBound = 0;

    public string GetSymbol() => _symbol;
    public double GetLowerBound() => _lowerBound;
    public double GetUpperBound() => _upperBound;

    public void SetUpperBound(double value) {
        _upperBound = value;
    }
    public void SetLowerBound(double value) {
        _lowerBound = value;
    }
    public void SetSymbol(string value) {
        _symbol = value;
    }
}
