using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
        public ListView ListViewControl { get; }

        public string OrderBy;
        public bool OrderDescen;
        private bool IsBusy;

        public MainViewModel(ListView ListViewControl)
        {
            ListLoad = false;

            GetProducts("id", false);

            this.ListViewControl = ListViewControl;
        }
        public MainViewModel()
        {
            ListLoad = false;

            GetProducts("id", false);
        }
        public ICommand SelectMultiple => new RelayCommand(new Action(() => SelectMulti()));
        //public ICommand ClearFilter => new RelayCommand(new Action(() => ClearFilter()));
        //public ICommand OrderDescendent => new RelayCommand(new Action(() => OrderDescendent()));
        //public ICommand OrderAscendant => new RelayCommand(new Action(() => OrderAscendant()));
        //public ICommand OrderByName => new RelayCommand(new Action(() => OrderByName()));
        //public ICommand OrderByPrice => new RelayCommand(new Action(() => OrderByPrice()));
        //public ICommand OrderByStock => new RelayCommand(new Action(() => OrderByStock()));
        //public ICommand AddProduct => new RelayCommand(new Action(() => AddProduct()));
        public ICommand UpdateList => new RelayCommand(new Action(() => GetProducts(OrderBy, OrderDescen)));
        public ICommand DeleteCommand => new RelayCommand(new Action(async () => await DeleteAsync()));
        private async Task DeleteAsync()
        {
            try
            {
                IList<object> itemsSelected = ListViewControl.SelectedItems;
                if (itemsSelected.Count > 0)
                {
                    string itemsS = itemsSelected.Count.ToString();
                    string content;
                    if (itemsSelected.Count == 1)
                    {
                        content = $"Esta seguro de eliminar el registro?\nSe dejaran de seguir los productos relacionados con la tienda";
                    }
                    else
                    {
                        content = $"Esta seguro de eliminar los {itemsS} registros?\nSe dejaran de seguir los productos relacionados con las tiendas";
                    }
                    ContentDialog deleteFileDialog = new ContentDialog
                    {
                        Title = "Delete Stores",
                        Content = content,
                        PrimaryButtonText = "Delete",
                        CloseButtonText = "Cancel"
                    };
                    ContentDialogResult result = await deleteFileDialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        foreach (var item in itemsSelected)
                        {
                            ProductInfo data = (ProductInfo)item;
                            int data1 = data.ID_PRODUCT;

                            await App.PriceTrackerService.DeleteProductAsync(data1);
                        }
                        GetProducts();
                        HideButtons();
                    }
                    else if (result == ContentDialogResult.None)
                    {
                        HideButtons();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
            ListViewControl.SelectedItem = null;
            SelectMultipleIsEnabled = false;
            ListViewControl.SelectionMode = ListViewSelectionMode.Single;
            IsCheckedAllVisibility = Visibility.Collapsed;
            DeleteCommandVisibility = Visibility.Collapsed;
        }
        private void ShowButtons()
        {
            SelectMultipleIsEnabled = true;
            ListViewControl.SelectionMode = ListViewSelectionMode.Multiple;
            IsCheckedAllVisibility = Visibility.Visible;
            DeleteCommandVisibility = Visibility.Visible;
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
        public async void GetProducts(string order = "id", bool Ascendant = false)
        {
            IsBusy = true;
            try
            {
                List<ProductInfo> lista = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                ListViewControl.Items.Clear();
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
            finally
            {
                IsBusy = false;
            }
        }
    }
}
