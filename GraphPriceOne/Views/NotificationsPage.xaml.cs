using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.ViewModels;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace GraphPriceOne.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class NotificationsPage : Page
    {
        private ProductDetailsViewModel selectors;

        public NotificationsPage()
        {
            this.InitializeComponent();
            DataContext = new NotificationsViewModel();
            selectors = new ProductDetailsViewModel();
        }
        private void NotificationsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListNotifications.SelectionMode == ListViewSelectionMode.Single && ListNotifications.SelectedItem != null)
            {
                NotificationsModel obj = (NotificationsModel)ListNotifications.SelectedItem;
                selectors.SelectedProduct = obj.PRODUCT_ID;
                NavigationService.Navigate(typeof(ProductDetailsPage));
            }
        }
        private void ListViewNotifications_RefreshRequested(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {

        }
    }
}
