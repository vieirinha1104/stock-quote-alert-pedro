namespace stock_quote_alert_pedro.Entities;
public class Config {
    public string? SmtpHost { get; set; }
    public string? HostEmail { get; set; }
    public string? HostPassword { get; set; }
    public string? EmailTo { get; set; }
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;

    public int CoolDown { get; set; } = 300; // default. 5 minutos
    public bool StopProgramIfMarketIsClosed { get; set; } = false;

}
