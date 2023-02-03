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

        public static string GetMetaValue(HtmlNode DocumentNode, string selector, string attributeName)
        {
            HtmlNode metaNode = DocumentNode.QuerySelector(selector);
            return metaNode.GetAttributeValue(attributeName, string.Empty);
        }

        public static string GetMetaTitle(HtmlNode DocumentNode)
        {
            return GetMetaValue(DocumentNode, "head > title", "InnerHtml");
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
