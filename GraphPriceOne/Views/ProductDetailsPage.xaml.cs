using System;

using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GraphPriceOne.Views
{
    public sealed partial class ProductDetailsPage : Page
    {
        public ProductDetailsViewModel ViewModel { get; } = new ProductDetailsViewModel();

        // TODO WTS: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ProductDetailsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
