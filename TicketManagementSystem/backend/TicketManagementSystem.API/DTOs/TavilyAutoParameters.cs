/// <summary>
/// Parámetros automáticos utilizados en la búsqueda de Tavily.
/// </summary>
public class TavilyAutoParameters
{
    /// <summary>
    /// Tema de la búsqueda.
    /// </summary>
    public string Topic { get; set; } = string.Empty;

    /// <summary>
    /// Profundidad de búsqueda.
    /// </summary>
    public string SearchDepth { get; set; } = string.Empty;
}