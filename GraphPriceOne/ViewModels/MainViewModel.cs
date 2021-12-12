using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.Views;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ListView ListViewControl { get; set; }
        public ObservableCollection<ProductInfo> ListViewCollection { get; set; }

        public string OrderBy;
        private List<ProductInfo> lista2;
        public bool OrderDescen;
        private bool IsBusy;

        public MainViewModel(ListView ListViewControl)
        {
            ListLoad = false;

            ListViewCollection = new ObservableCollection<ProductInfo>();
            _ = GetProductsAsync("id", false);
            ShowMessageFirstProduct();

            this.ListViewControl = ListViewControl;
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        public MainViewModel()
        {
            ListLoad = false;

            //GetProductsAsync("id", false);
        }
        public ICommand SelectMultipleCommand => new RelayCommand(new Action(() => SelectMulti()));
        public ICommand ClearFilterCommand => new RelayCommand(new Action(async () => await GetProductsAsync("id", false)));
        public ICommand OrderDescendentCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, false)));
        public ICommand OrderAscendantCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, true)));
        public ICommand OrderByNameCommand => new RelayCommand(new Action(async () => await GetProductsAsync("name", OrderDescen)));
        public ICommand OrderByPriceCommand => new RelayCommand(new Action(async () => await GetProductsAsync("price", OrderDescen)));
        public ICommand OrderByStockCommand => new RelayCommand(new Action(async () => await GetProductsAsync("stock", OrderDescen)));
        public ICommand AddProductCommand => new RelayCommand(new Action(async () => await AddProductAsync()));

        private async Task AddProductAsync()
        {
            HideMessageFirstProduct();
            ProductInfo item = new ProductInfo()
            {
                productName = "htx 1060"
            };
            await App.PriceTrackerService.AddProductAsync(item);
            await GetProductsAsync(OrderBy, OrderDescen);
        }

        public ICommand UpdateListCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, OrderDescen)));
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
                        await GetProductsAsync();
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
            IsCheckBoxChecked = false;
            ListViewControl.SelectedItem = null;
            SelectMultipleIsEnabled = false;
            ListViewControl.SelectionMode = ListViewSelectionMode.Single;
            IsCheckedAllVisibility = Visibility.Collapsed;
            DeleteCommandVisibility = Visibility.Collapsed;
        }
        private void ShowButtons()
        {
            IsCheckBoxChecked = false;
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
        public async Task GetProductsAsync(string order = "id", bool Ascendant = false)
        {
            IsBusy = true;
            try
            {
                ListViewCollection.Clear();
                List<ProductInfo> lista = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                List<ProductInfo> ListProduct = new List<ProductInfo>();

                if (lista != null && lista.Count != 0)
                {
                    HideMessageFirstProduct();

                    foreach (var item in lista)
                    {
                        ListProduct.Add(new ProductInfo
                        {
                            ID_PRODUCT = item.ID_PRODUCT,
                            productName = item.productName,
                            productUrl = item.productUrl,
                            productDescription = item.productDescription,
                            PriceTag = item.PriceTag,
                            priceCurrency = item.priceCurrency,
                            shippingPrice = item.shippingPrice,
                            shippingCurrency = item.shippingCurrency,
                            stock = item.stock
                        });
                    }

                    if (order == "name" && Ascendant == false)
                    {
                        OrderBy = "name";
                        lista2 = ListProduct.OrderByDescending(o => o.productName).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "name" && Ascendant == true)
                    {
                        OrderBy = "name";
                        lista2 = ListProduct.OrderBy(o => o.productName).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "id" && Ascendant == false)
                    {
                        OrderBy = "id";
                        lista2 = ListProduct.OrderByDescending(o => o.ID_PRODUCT).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "id" && Ascendant == true)
                    {
                        OrderBy = "id";
                        lista2 = ListProduct.OrderBy(o => o.ID_PRODUCT).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "price" && Ascendant == false)
                    {
                        OrderBy = "price";
                        lista2 = ListProduct.OrderByDescending(o => o.PriceTag).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "price" && Ascendant == true)
                    {
                        OrderBy = "price";
                        lista2 = ListProduct.OrderBy(o => o.PriceTag).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "stock" && Ascendant == false)
                    {
                        OrderBy = "stock";
                        lista2 = ListProduct.OrderByDescending(o => o.stock).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "stock" && Ascendant == true)
                    {
                        OrderBy = "stock";
                        lista2 = ListProduct.OrderBy(o => o.stock).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else
                    {
                        OrderBy = "id";
                        lista2 = ListProduct.OrderByDescending(o => o.ID_PRODUCT).ToList();
                        foreach (var item in lista2)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                }
                else
                {
                    ShowMessageFirstProduct();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
