using System;

using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Views
{
    public sealed partial class AddStorePage : Page
    {
        public AddStoreViewModel ViewModel { get; } = new AddStoreViewModel();

        public AddStorePage()
        {
            InitializeComponent();
        }
    }
}
