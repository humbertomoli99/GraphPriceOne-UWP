using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GraphPriceOne.Views
{
    public sealed partial class WhatsNewDialog : ContentDialog
    {
        public WhatsNewDialog()
        {
            // TODO WTS: Update the contents of this dialog every time you release a new version of the app
            RequestedTheme = (App.Window.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
        }
    }
}
