using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraphPriceOne.Helpers
{
    public class MetaDataHelper
    {
        public static async Task DownloadFaviconAsync(string url)
        {
            // Crea una instancia de CoreWebView2
            WebView2 webView = new WebView2();
            try
            {
                // Intenta descargar el favicon directamente a partir de la URL https://www.example.com/favicon.ico
                var faviconUrl = $"https://{new Uri(url).Host}/favicon.ico";
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(faviconUrl);

                // Si la descarga directa falla, busca el elemento "link" con "rel=shortcut icon"
                if (!response.IsSuccessStatusCode)
                {
                    // Buscar el elemento "link" con "rel=shortcut icon"
                    var result = await webView.ExecuteScriptAsync($"document.querySelector('link[rel*=\'icon\']').href");
                    faviconUrl = result.ToString();

                    // Si no se encuentra, muestra un error
                    if (string.IsNullOrEmpty(faviconUrl))
                    {
                        Console.WriteLine("No se encontró ningún favicon en la página");
                        return;
                    }
                }

                // Determinar el formato de descarga basándose en la extensión original
                var format = GetFaviconFormat(faviconUrl);

                // Si el formato no es soportado, muestra un error
                if (format == "")
                {
                    Console.WriteLine("Formato de favicon no soportado");
                    return;
                }

                // Descargar el favicon
                response = await client.GetAsync(faviconUrl);
                var favicon = await response.Content.ReadAsByteArrayAsync();

                // Guardar el favicon en el disco local
                System.IO.File.WriteAllBytes($"favicon.{format}", favicon);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Liberar la instancia de CoreWebView2
                webView.Close();
            }
        }

        private static string GetFaviconFormat(string faviconUrl)
        {
            // Obtener la extensión del archivo del favicon
            string extension = Path.GetExtension(faviconUrl).ToLower().TrimStart('.');

            // Comprobar si el formato es soportado
            if (new List<string> { "ico", "png", "svg", "gif", "jpg" }.Contains(extension))
            {
                return extension;
            }
            else
            {
                // Si el formato no es soportado, lanzar una excepción
                throw new Exception("Formato de favicon no soportado");
            }
        }
        /// Obtiene el valor de un atributo de un nodo HTML específico.
        /// </summary>
        /// <param name="documentNode">El nodo raíz del documento HTML.</param>
        /// <param name="selector">El selector CSS utilizado para seleccionar el nodo deseado.</param>
        /// <param name="attributeName">El nombre del atributo que se desea obtener.</param>
        /// <returns>El valor del atributo o una cadena vacía en caso de no existir.</returns>
        public static string GetMetaValue(HtmlNode documentNode, string selector, string attributeName)
        {
            // Selecciona el nodo específico utilizando el selector CSS
            HtmlNode metaNode = documentNode.QuerySelector(selector);

            // Inicializa una variable para almacenar el resultado
            string result = string.Empty;

            // Si se quiere obtener el contenido HTML interno del nodo
            if (attributeName == "innerHTML")
            {
                // Asigna el contenido interno del nodo al resultado
                result = metaNode?.InnerHtml;
            }
            else
            {
                // Asigna el valor del atributo al resultado
                result = metaNode?.GetAttributeValue(attributeName, string.Empty);
            }

            // Devuelve el resultado o una cadena vacía en caso de ser nulo
            return result ?? string.Empty;
        }

        public static string GetMetaTitle(HtmlNode DocumentNode)
        {
            return GetMetaValue(DocumentNode, "head > title", "innerHTML");
        }

        public static string GetMetaDescription(HtmlNode DocumentNode)
        {
            return GetMetaValue(DocumentNode, "head > meta[name='description']", "content");
        }

        public static string GetMetaImage(HtmlNode DocumentNode)
        {
            return GetMetaValue(DocumentNode, "head > meta[property='og:image']", "content");
        }


    }
}
