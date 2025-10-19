using Elasticsearch.Net;
using System.Text.Json;

namespace ElasticSearchPOC.Services;

public class LoggingService
{
    private readonly ElasticLowLevelClient _client;
    private const string LogIndex = "application-logs";

    public LoggingService()
    {
        var settings = new ConnectionConfiguration(new Uri("http://localhost:9200"));
        _client = new ElasticLowLevelClient(settings);
    }

    public async Task LogAsync(string level, string message, string source = "application")
    {
        var logEntry = new
        {
            timestamp = DateTime.UtcNow,
            level,
            message,
            source,
            application = "ElasticSearchPOC"
        };

        var json = JsonSerializer.Serialize(logEntry);
        await _client.IndexAsync<StringResponse>(LogIndex, PostData.String(json));
    }

    public async Task LogErrorAsync(Exception ex, string context = "")
    {
        await LogAsync("ERROR", $"{context}: {ex.Message}", ex.Source ?? "unknown");
    }

    public async Task LogInfoAsync(string message, string source = "application")
    {
        await LogAsync("INFO", message, source);
    }
}
