﻿using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Library
{
    public class Dialogs
    {
        public static async Task ExceptionDialog(Exception ex)
        {
            ContentDialog ExcepcionMessage = new ContentDialog()
            {
                Title = "Exception",
                PrimaryButtonText = "Ok",
                Content = ex.ToString()
            };
            await ExcepcionMessage.ShowAsync();
        }
    }
}
