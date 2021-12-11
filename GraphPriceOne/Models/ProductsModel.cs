using GraphPriceOne.Core.Models;
using GraphPriceOne.ViewModels;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Models
{
    public class ProductsModel : PropertyChangedNotification
    {
        public Visibility IsCheckedAllVisibility
        {
            get { return GetValue(() => IsCheckedAllVisibility); }
            set { SetValue(() => IsCheckedAllVisibility, value); }
        }
        public Visibility DeleteCommandVisibility
        {
            get { return GetValue(() => DeleteCommandVisibility); }
            set { SetValue(() => DeleteCommandVisibility, value); }
        }
        public Visibility FirstProductVisibility
        {
            get { return GetValue(() => FirstProductVisibility); }
            set { SetValue(() => FirstProductVisibility, value); }
        }
        public Visibility CommandBarVisibility
        {
            get { return GetValue(() => CommandBarVisibility); }
            set { SetValue(() => CommandBarVisibility, value); }
        }
        public Visibility ListProductsVisibility { get; set; }
    }
}
