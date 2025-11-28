/// <summary>
/// Servicio para integrar con la API de Tavily para búsquedas web.
/// </summary>
public interface ITavilyService
{
    /// <summary>
    /// Realiza una búsqueda utilizando la API de Tavily.
    /// </summary>
    /// <param name="query">La consulta de búsqueda.</param>
    /// <returns>Los resultados de la búsqueda.</returns>
    Task<TavilyResponse> SearchAsync(string query);
}