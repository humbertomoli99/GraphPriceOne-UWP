using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace GraphPriceOne.Library
{
    public class Dialogs
    {
        public static async Task ExceptionDialog(Exception ex)
        {
            ContentDialog ExcepcionMessage = new ContentDialog()
            {
                Title = "Exception",
                PrimaryButtonText = "Ok",
                Content = ex.ToString()
            };
            await this.SetContentDialogRoot(ExcepcionMessage).ShowAsync();
        }
                    private ContentDialog SetContentDialogRoot(ContentDialog contentDialog)
                    {
                        if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
                        {
                            contentDialog.XamlRoot = this.Content.XamlRoot;
                        }
                        return contentDialog;
                    }
    }
}
