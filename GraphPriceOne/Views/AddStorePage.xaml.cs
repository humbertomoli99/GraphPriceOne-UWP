﻿using System;

using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Views
{
    public sealed partial class AddStorePage : Page
    {
        public AddStorePage()
        {
            InitializeComponent();
            Object[] campos = { StoreName, StoreURL };
            DataContext = new AddStoreViewModel(campos);
        }
    }
}
