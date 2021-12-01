using GraphPriceOne.Core.Models;
using GraphPriceOne.Core.Services;
using GraphPriceOne.Library;
using Microsoft.Toolkit.Mvvm.ComponentModel;
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
    public class MainViewModel : ObservableObject
    {
        private ProductInfo ProductInfo;
        public ProductPhotos ProductImages;
        private History ProductHistory;
        private bool SelectMultipleIsEnabled;
        private ListView _ListView;
        public SQLiteConnection _sqlite;


        public Visibility isCheckedAllVisibility { get; private set; }
        public Visibility DeleteStoreVisibility { get; private set; }
        public Visibility FirstProductVisibility { get; private set; }
        public Visibility ListProductsVisibility { get; private set; }
        public Visibility CommandBarVisibility { get; set; }
        public bool ListLoad { get; }

        public MainViewModel(ListView Lista)
        {
            ListLoad = false;
            _ListView = Lista;
            GetProducts();
            HideMessageFirstProduct();

        }
        public MainViewModel()
        {
            ListLoad = false;
        }
        public ICommand SelectMultiple
        {
            get
            {
                return new CommandHandler(() => SelectMulti());
            }
        }
        private void ShowMessageFirstProduct()
        {
            FirstProductVisibility = Windows.UI.Xaml.Visibility.Visible;
            ListProductsVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            CommandBarVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        private void HideMessageFirstProduct()
        {
            FirstProductVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            ListProductsVisibility = Windows.UI.Xaml.Visibility.Visible;
            CommandBarVisibility = Windows.UI.Xaml.Visibility.Visible;
        }
        private void HideButtons()
        {
            SelectMultipleIsEnabled = false;
            isCheckedAllVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            DeleteStoreVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        private void ShowButtons()
        {
            SelectMultipleIsEnabled = true;
            isCheckedAllVisibility = Windows.UI.Xaml.Visibility.Visible;
            DeleteStoreVisibility = Windows.UI.Xaml.Visibility.Visible;
        }
        private void SelectMulti()
        {
            bool IsMultiSelect = _ListView.IsMultiSelectCheckBoxEnabled;
            int itemsSelected = _ListView.SelectedItems.Count;
            int AllItems = _ListView.Items.Count;

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
