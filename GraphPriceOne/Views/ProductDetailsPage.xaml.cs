using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Views
{
    public sealed partial class ProductDetailsPage : Page
    {
        //public ProductDetailsViewModel ViewModel { get; } = new ProductDetailsViewModel();

        // TODO WTS: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ProductDetailsPage()
        {
            InitializeComponent();
            DataContext = new ProductDetailsViewModel();
        }

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    //await ViewModel.LoadDataAsync();
        //}

        private void FlipView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.flip.Height = this.ActualWidth * 0.5625;
        }
    }
}
