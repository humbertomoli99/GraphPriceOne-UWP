using System;

using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GraphPriceOne.Views
{
    public sealed partial class StoresPage : Page
    {
        public StoresViewModel ViewModel { get; } = new StoresViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on StoresPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public StoresPage()
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
