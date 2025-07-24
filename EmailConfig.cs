public class EmailConfig {
    public string? SmtpHost { get; set; }
    public string? HostEmail { get; set; }
    public string? HostPassword { get; set; }
    public string? EmailTo { get; set; }

    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
}
