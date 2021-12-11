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
        public Visibility DeleteStoreVisibility
        {
            get { return GetValue(() => DeleteStoreVisibility); }
            set { SetValue(() => DeleteStoreVisibility, value); }
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
        public ICommand ClearFilter { get; set; }
        public ICommand OrderDescendent { get; set; }
        public ICommand OrderAscendant { get; set; }
        public ICommand OrderByName { get; set; }
        public ICommand OrderByPrice { get; set; }
        public ICommand OrderByStock { get; set; }
        public ICommand UpdateList { get; set; }
        public ICommand AddProduct { get; set; }
        public ICommand DeleteStore { get; set; }
    }
}
