using GraphPriceOne.Core.Models;
using GraphPriceOne.Core.Services;
using GraphPriceOne.Library;
using GraphPriceOne.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.ViewModels
{
    public class MainViewModel : ProductsModel
    {
        public bool SelectMultipleIsEnabled { get; set; }
        public bool ListLoad { get; }


        public ListView ListProducts;

        private string OrderBy;
        private bool OrderDescen;
        public MainViewModel(ListView Lista)
        {
            ListLoad = false;
            ListProducts = Lista;

            OrderBy = "id";
            OrderDescen = false;

            GetProducts(OrderBy, OrderDescen);

            HideMessageFirstProduct();

            SelectMultiple = new RelayCommand(new Action(() => SelectMulti()));
        }
        public MainViewModel()
        {
            ListLoad = false;
        }
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
        private void SelectMulti()
        {
            bool IsMultiSelect = ListProducts.IsMultiSelectCheckBoxEnabled;
            int itemsSelected = ListProducts.SelectedItems.Count;
            int AllItems = ListProducts.Items.Count;

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
        private async void GetProducts(string order = null, bool Ascendant = false)
        {
            try
            {
                List<ProductInfo> lista = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();

                if (lista != null && lista.Count != 0)
                {
                    HideMessageFirstProduct();
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
