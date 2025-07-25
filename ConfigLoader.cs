using System.Text.Json;

namespace stock_quote_alert_pedro;
public static class ConfigLoader {
    public static Config LoadEmailConfig(string path) {
        try {
            string json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<Config>(json);

            if (config is null)
                throw new InvalidOperationException("Failed to deserialize EmailConfig from JSON.");

            return config;
        } catch (Exception ex) {
            Console.WriteLine($"Error loading email config: {ex.Message}");
            throw;
        }
    }
}
