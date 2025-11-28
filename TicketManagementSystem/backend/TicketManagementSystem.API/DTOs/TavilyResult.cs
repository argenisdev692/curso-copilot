/// <summary>
/// Representa un resultado individual de búsqueda de Tavily.
/// </summary>
public class TavilyResult
{
    /// <summary>
    /// Título del resultado.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// URL del resultado.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Contenido resumido del resultado.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Puntaje de relevancia.
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    /// Contenido crudo si está disponible.
    /// </summary>
    public string? RawContent { get; set; }

    /// <summary>
    /// URL del favicon.
    /// </summary>
    public string? Favicon { get; set; }
}