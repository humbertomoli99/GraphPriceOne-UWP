using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.ViewModels
{
    public class MainViewModel : ProductsModel
    {
        public bool SelectMultipleIsEnabled { get; set; }
        public bool ListLoad { get; }
        public ListView ListViewControl { get; }

        private string OrderBy;
        private bool OrderDescen;
        public MainViewModel(ListView ListViewControl)
        {
            ListLoad = false;

            GetProducts("id", false);

            this.ListViewControl = ListViewControl;
        }
        public ICommand SelectMultiple => new RelayCommand(new Action(() => SelectMulti()));
        //public ICommand ClearFilter => new RelayCommand(new Action(() => ClearFilter()));
        //public ICommand OrderDescendent => new RelayCommand(new Action(() => OrderDescendent()));
        //public ICommand OrderAscendant => new RelayCommand(new Action(() => OrderAscendant()));
        //public ICommand OrderByName => new RelayCommand(new Action(() => OrderByName()));
        //public ICommand OrderByPrice => new RelayCommand(new Action(() => OrderByPrice()));
        //public ICommand OrderByStock => new RelayCommand(new Action(() => OrderByStock()));
        //public ICommand UpdateList => new RelayCommand(new Action(() => UpdateList()));
        //public ICommand AddProduct => new RelayCommand(new Action(() => AddProduct()));
        //public ICommand DeleteStore => new RelayCommand(new Action(() => DeleteStore()));
        private void ShowMessageFirstProduct()
        {
            FirstProductVisibility = Visibility.Visible;
            ListProductsVisibility = Visibility.Collapsed;
            CommandBarVisibility = Visibility.Collapsed;
        }
        private void HideMessageFirstProduct()
        {
            HideButtons();
            FirstProductVisibility = Visibility.Collapsed;
            ListProductsVisibility = Visibility.Visible;
            CommandBarVisibility = Visibility.Visible;
        }
        private void HideButtons()
        {
            SelectMultipleIsEnabled = false;
            IsCheckedAllVisibility = Visibility.Collapsed;
            DeleteStoreVisibility = Visibility.Collapsed;
        }
        private void ShowButtons()
        {
            SelectMultipleIsEnabled = true;
            IsCheckedAllVisibility = Visibility.Visible;
            DeleteStoreVisibility = Visibility.Visible;
        }
        public void SelectMulti()
        {
            bool IsMultiSelect = ListViewControl.IsMultiSelectCheckBoxEnabled;
            int itemsSelected = ListViewControl.SelectedItems.Count;
            int AllItems = ListViewControl.Items.Count;

            if (AllItems > 0)
            {
                if (SelectMultipleIsEnabled == false)
                {
                    ShowButtons();
                }
                else if (SelectMultipleIsEnabled == true)
                {
                    HideButtons();
                }
            }
        }
        private async void GetProducts(string order = "id", bool Ascendant = false)
        {
            try
            {
                List<ProductInfo> lista = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();

                if (lista != null && lista.Count != 0)
                {
                    HideMessageFirstProduct();

                    foreach (var item in lista)
                    {
                        ListViewControl.Items.Add(item);
                    }
                }
                else
                {
                    ShowMessageFirstProduct();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
