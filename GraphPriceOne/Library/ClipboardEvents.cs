using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace GraphPriceOne.Library
{
    public class ClipboardEvents
    {
        /// <summary>
        /// Obtiene el texto del portapapeles de manera asíncrona.
        /// </summary>
        /// <returns>Devuelve el texto del portapapeles como una tarea completada.</returns>
        public static async Task<string> GetClipboardTextAsync()
        {
            // Obtiene el contenido actual del portapapeles
            var content = Clipboard.GetContent();

            // Verifica si el contenido contiene texto
            return content.Contains(StandardDataFormats.Text)
                // Si contiene texto, lo devuelve como tarea completada
                ? await content.GetTextAsync()
                // Si no contiene texto, devuelve una cadena vacía como tarea completada
                : await Task.FromResult(string.Empty);
        }
    }
}
