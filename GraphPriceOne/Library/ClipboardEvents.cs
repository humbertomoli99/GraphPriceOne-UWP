using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace GraphPriceOne.Library
{
    public class ClipboardEvents
    {
        public static async Task<string> OutputClipboardTextAsync()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                string text = await dataPackageView.GetTextAsync();
                return text;
            }

            return await Task.FromResult(String.Empty);
        }
    }
}
