using System;

using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
