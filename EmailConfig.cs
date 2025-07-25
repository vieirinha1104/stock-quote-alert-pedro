public class EmailConfig {
    public string? SmtpHost { get; set; }
    public string? HostEmail { get; set; }
    public string? HostPassword { get; set; }
    public string? EmailTo { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; }

    public int CoolDown { get; set; } = 300; // default. 5 minutos
}
