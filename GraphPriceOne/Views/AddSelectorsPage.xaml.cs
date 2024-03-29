﻿using GraphPriceOne.ViewModels;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace GraphPriceOne.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class AddSelectorsPage : Page
    {
        public AddSelectorsPage()
        {
            this.InitializeComponent();
            DataContext = new AddSelectorsViewModel();
        }
    }
}
