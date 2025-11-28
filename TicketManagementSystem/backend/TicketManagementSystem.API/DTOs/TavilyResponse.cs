/// <summary>
/// Respuesta completa de la API de Tavily.
/// </summary>
public class TavilyResponse
{
    /// <summary>
    /// La consulta de búsqueda realizada.
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Respuesta generada por IA si está disponible.
    /// </summary>
    public string? Answer { get; set; }

    /// <summary>
    /// Lista de imágenes relacionadas.
    /// </summary>
    public List<object> Images { get; set; } = new();

    /// <summary>
    /// Lista de resultados de búsqueda.
    /// </summary>
    public List<TavilyResult> Results { get; set; } = new();

    /// <summary>
    /// Parámetros automáticos utilizados.
    /// </summary>
    public TavilyAutoParameters? AutoParameters { get; set; }

    /// <summary>
    /// Tiempo de respuesta en segundos.
    /// </summary>
    public string ResponseTime { get; set; } = string.Empty;

    /// <summary>
    /// ID único de la solicitud.
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
}