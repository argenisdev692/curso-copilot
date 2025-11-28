using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

/// <summary>
/// Implementación del servicio de Tavily.
/// </summary>
public class TavilyService : ITavilyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TavilySettings _settings;
    private readonly ILogger<TavilyService> _logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    public TavilyService(IHttpClientFactory httpClientFactory, IOptions<TavilySettings> settings, ILogger<TavilyService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TavilyResponse> SearchAsync(string query)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.tavily.com/search");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            var body = new { query };
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TavilyResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _logger.LogInformation("Búsqueda realizada exitosamente con Tavily para query: {Query}", query);
            return result ?? new TavilyResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al realizar búsqueda con Tavily para query: {Query}", query);
            throw;
        }
    }
}