using GraphPriceOne.Library;
using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GraphPriceOne.Views
{
    public sealed partial class ExportPage : Page
    {
        public ExportViewModel ViewModel { get; } = new ExportViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ExportPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public ExportPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ExportData.ExportDataCsv();
        }
    }
}
