using GraphPriceOne.Core.Models;
using GraphPriceOne.Library;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.Views;
using HtmlAgilityPack;
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
        public ListView ListViewControl { get; set; }
        public ObservableCollection<ProductInfo> ListViewCollection { get; set; }
        public History ProductHistory { get; private set; }
        public ProductPhotos ProductImages { get; private set; }
        public ProductInfo Product { get; private set; }

        private List<ProductInfo> OrderedList;
        public string OrderBy;
        public bool OrderDescen;

        public MainViewModel(ListView ListViewControl)
        {
            IsBusy = false;

            ListViewCollection = new ObservableCollection<ProductInfo>();
            _ = GetProductsAsync("id", false);
            ShowMessageFirstProduct();

            this.ListViewControl = ListViewControl;
        }
        public MainViewModel()
        {

        }
        public ICommand SelectMultipleCommand => new RelayCommand(new Action(() => SelectMulti()));
        public ICommand ClearFilterCommand => new RelayCommand(new Action(async () => await GetProductsAsync()));
        public ICommand OrderDescendentCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, false)));
        public ICommand OrderAscendantCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, true)));
        public ICommand OrderByNameCommand => new RelayCommand(new Action(async () => await GetProductsAsync("name", OrderDescen)));
        public ICommand OrderByPriceCommand => new RelayCommand(new Action(async () => await GetProductsAsync("price", OrderDescen)));
        public ICommand OrderByStockCommand => new RelayCommand(new Action(async () => await GetProductsAsync("stock", OrderDescen)));
        public ICommand AddProductCommand => new RelayCommand(new Action(async () => await AddProductAsync()));
        public ICommand UpdateListCommand => new RelayCommand(new Action(async () => await GetProductsAsync(OrderBy, OrderDescen)));
        public ICommand DeleteCommand => new RelayCommand(new Action(async () => await DeleteAsync()));
        private async Task AddProductAsync()
        {
            IsBusy = true;
            try
            {
                string url = await ClipboardEvents.OutputClipboardTextAsync();

                var Products = await App.PriceTrackerService.GetProductsAsync();
                var query = Products.Where(s => s.productUrl.Equals(url))?.ToList();
                var Stores = await App.PriceTrackerService.GetStoresAsync();
                var UrlShop = Stores.Where(s => url.Contains(s.startUrl))?.ToList();

                //url no valida
                //validar url valida para envio masivo de productos
                if (!TextBoxEvent.IsValidURL(url))
                {
                    ContentDialog dialogError = new ContentDialog()
                    {
                        Title = "Add a new product",
                        PrimaryButtonText = "OK, THANKS",
                        CloseButtonText = "CANCEL",
                        Content = "Copy a URL to begin\n" + "Copy the URL of the product, then select Add Product to start tracking the product's price."
                    };
                    ContentDialogResult result = await dialogError.ShowAsync();
                }
                else
                {
                    //no sitemap
                    if (UrlShop.Count == 0 || UrlShop == null)
                    {
                        ContentDialog dialogOk = new ContentDialog()
                        {
                            Title = "No selectors assigned to Store",
                            PrimaryButtonText = "OK",
                            SecondaryButtonText = "MASS SHIPPING",
                            CloseButtonText = "CANCEL",
                            Content = "The store has no assigned sectors."
                        };
                        ContentDialogResult result = await dialogOk.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            NavigationService.Navigate(typeof(AddStorePage));
                        }
                        else if (result == ContentDialogResult.Secondary)
                        {
                            // obtener todas las url de la pagina, y pasarla por el filtro si existe la tienda con selectores
                            await MassShipping(url);
                        }
                    }
                    //crear un if por si el sitemap no tiene selectores
                    else
                    {
                        //ya registrado
                        if (query.Count > 0)
                        {
                            ContentDialog dialogOk = new ContentDialog()
                            {
                                Title = "This product is already registered",
                                PrimaryButtonText = "OK",
                                Content = "The product is registered and will continue to be tracked."
                            };
                            ContentDialogResult result = await dialogOk.ShowAsync();
                        }
                        else
                        {
                            HtmlNode HtmlUrl = await ScrapingDate.LoadPageAsync(url);
                            ScrapingDate.EnlaceImage icon = ScrapingDate.GetMetaIcon(HtmlUrl);

                            var id_sitemap = UrlShop.First().ID_STORE;
                            var Selectores = await App.PriceTrackerService.GetSelectorsAsync();
                            var SitemapSelectors = Selectores.Where(s => s.ID_SELECTOR.Equals(id_sitemap))?.ToList()?.First();

                            //string urlimage = ScrapingDate.DownloadImage(url, imagen, @"\Products\","holaxd");


                            Product = new ProductInfo()
                            {
                                ID_STORE = id_sitemap,
                                productName = ScrapingDate.GetTitle(HtmlUrl, SitemapSelectors.Title),
                                productUrl = url,
                                productDescription = ScrapingDate.GetDescription(HtmlUrl, SitemapSelectors.Description, SitemapSelectors.DescriptionGetAttribute),
                                stock = ScrapingDate.GetStock(HtmlUrl, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                                PriceTag = ScrapingDate.GetPrice(HtmlUrl, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                                //Image = ScrapingDate.DownloadImage(url, imagen, @"\Products\", LastID.ToString()),
                                shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                            };

                            var shipping = Product.shippingCurrency + " " + Product.shippingPrice;
                            if (Product.shippingPrice == 0)
                            {
                                shipping = "Free shipping";
                            }
                            else if (Product.shippingCurrency == null)
                            {
                                shipping = "$" + Product.shippingPrice;
                            }

                            var currency = Product.priceCurrency;
                            if (Product.priceCurrency == null)
                            {
                                currency = "$";
                            }

                            var content = Product.productName + "\n\n" +
                                "Price: " + currency + Product.PriceTag + "\n" +
                                "Shipping: " + shipping + "\n" +
                                "Store ID: " + Product.ID_STORE;

                            ContentDialog dialogOk = new ContentDialog()
                            {
                                Title = "Add a new product",
                                PrimaryButtonText = "ADD PRODUCT",
                                SecondaryButtonText = "MASS SHIPPING",
                                CloseButtonText = "CANCEL",
                                Content = content
                            };
                            ContentDialogResult result = await dialogOk.ShowAsync();

                            if (result == ContentDialogResult.Primary)
                            {
                                await App.PriceTrackerService.AddProductAsync(Product);

                                var lastId = (Products.ToList().Count == 0) ? 1 : Products.ToList()[Products.ToList().Count - 1].ID_PRODUCT;

                                // si hay 0 items es 1;
                                //for para añadir todas las imagenes encontradas
                                List<string> imagen = ScrapingDate.GetUrlImage(HtmlUrl, SitemapSelectors.Images);

                                string[] imagenes = ScrapingDate.DownloadImage(url, imagen, @"\Products\", lastId.ToString());
                                if (imagenes != null)
                                {
                                    foreach (var item in imagenes)
                                    {
                                        ProductImages = new ProductPhotos()
                                        {
                                            PhotoSrc = item,
                                            ID_PRODUCT = lastId,

                                        };
                                        await App.PriceTrackerService.AddImageAsync(ProductImages);
                                    }
                                }

                                ProductHistory = new History()
                                {
                                    PRODUCT_ID = lastId,
                                    productDate = DateTime.UtcNow.ToString(),
                                    STORE_ID = id_sitemap,
                                    stock = ScrapingDate.GetStock(HtmlUrl, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                                    priceTag = ScrapingDate.GetPrice(HtmlUrl, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                                    shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                                };

                                await App.PriceTrackerService.AddHistoryAsync(ProductHistory);

                                HideMessageFirstProduct();
                                await GetProductsAsync(OrderBy, OrderDescen);
                            }
                            else if (result == ContentDialogResult.Secondary)
                            {
                                HideMessageFirstProduct();
                                await MassShipping(url);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                ContentDialog dialogOk1 = new ContentDialog()
                {
                    Title = "Exception",
                    PrimaryButtonText = "ok",
                    Content = ex.ToString()
                };
                await dialogOk1.ShowAsync();
            }
            IsBusy = false;
        }
        private async Task MassShipping(string url)
        {
            HtmlNode HtmlUrl = await ScrapingDate.LoadPageAsync(url);
            //obtener la url de productos masivos
            // obtener atributo href de todas las url
            //obtener todas las url de el html
            //crear un objeto con todas las url encontradas
            List<string> ListUrl = ScrapingDate.GetUrls(HtmlUrl);
            ListUrl = ListUrl.Distinct().ToList();
            IsBusy = true;
            List<string> ListValidUrl = new List<string>();
            foreach (var list in ListUrl)
            {
                var Sitemaps = await App.PriceTrackerService.GetStoresAsync();
                var ValidSitemaps = Sitemaps.Where(s => list.Contains(s.startUrl))?.ToList();
                //revisar cuales url son validas(que tienen store)
                if (ValidSitemaps.Count > 0)
                {
                    ListValidUrl.Add(list);
                }
            }
            foreach (var item in ListValidUrl)
            {
                IsBusy = true;
                await InsertProduct(item);
            }
            IsBusy = false;
            await GetProductsAsync(OrderBy, OrderDescen);
            //crear un objeto de las url validas
            //crear un for para añadir todas las urls con la validacion y 1 primer historial
        }
        public async Task InsertProduct(string url)
        {
            var Sitemaps = await App.PriceTrackerService.GetStoresAsync();
            var ValidSitemaps = Sitemaps.Where(s => url.Contains(s.startUrl))?.ToList();

            HtmlNode HtmlUrl1 = await ScrapingDate.LoadPageAsync(url);

            var id_sitemap = ValidSitemaps.First().ID_STORE;

            var Selectors = await App.PriceTrackerService.GetSelectorsAsync();
            var SitemapSelectors = Selectors.Where(s => s.ID_SELECTOR.Equals(id_sitemap)).ToList().First();
            // descarga de imagen provisional

            Product = new ProductInfo()
            {
                ID_STORE = id_sitemap,
                productName = ScrapingDate.GetTitle(HtmlUrl1, SitemapSelectors.Title),
                productUrl = url,
                productDescription = ScrapingDate.GetDescription(HtmlUrl1, SitemapSelectors.Description, SitemapSelectors.DescriptionGetAttribute),
                stock = ScrapingDate.GetStock(HtmlUrl1, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                PriceTag = ScrapingDate.GetPrice(HtmlUrl1, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl1, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute),
            };

            if (SitemapSelectors.TitleNotNull == 1 && Product.productName != null ||
                SitemapSelectors.TitleNotNull == 0)
            {
                if (SitemapSelectors.DescriptionNotNull == 1 && Product.productDescription != null ||
                    SitemapSelectors.DescriptionNotNull == 0)
                {
                    if (SitemapSelectors.ImagesNotNull == 1 && Product.Image != null || SitemapSelectors.ImagesNotNull == 0)
                    {
                        if (SitemapSelectors.PriceNotNull == 1 &&
                            Product.PriceTag != null || SitemapSelectors.PriceNotNull == 0)
                        {
                            if (SitemapSelectors.PriceCurrencyNotNull == 1 &&
                                Product.priceCurrency != null || SitemapSelectors.PriceCurrencyNotNull == 0)
                            {
                                if (SitemapSelectors.ShippingNotNull == 1 &&
                                    Product.shippingPrice != null || SitemapSelectors.ShippingNotNull == 0)
                                {
                                    if (SitemapSelectors.ShippingCurrencyNotNull == 1 &&
                                        Product.shippingCurrency != null || SitemapSelectors.ShippingCurrencyNotNull == 0)
                                    {
                                        if (SitemapSelectors.StockNotNull == 1 &&
                                            Product.stock != null || SitemapSelectors.StockNotNull == 0)
                                        {
                                            await App.PriceTrackerService.AddProductAsync(Product);

                                            var Products = await App.PriceTrackerService.GetProductsAsync();
                                            var lastId = (Products.ToList().Count == 0) ? 1 : Products.ToList()[Products.ToList().Count - 1].ID_PRODUCT;

                                            //for para añadir todas las imagenes encontradas
                                            List<string> imagen = ScrapingDate.GetUrlImage(HtmlUrl1, SitemapSelectors.Images);

                                            string[] imagenes = ScrapingDate.DownloadImage(url, imagen, @"\Products\", lastId.ToString());
                                            if (imagenes != null)
                                            {
                                                foreach (var item in imagenes)
                                                {
                                                    ProductImages = new ProductPhotos()
                                                    {
                                                        PhotoSrc = item,
                                                        ID_PRODUCT = lastId,

                                                    };
                                                    await App.PriceTrackerService.AddImageAsync(ProductImages);
                                                }
                                            }

                                            ProductHistory = new History()
                                            {
                                                PRODUCT_ID = lastId,
                                                productDate = DateTime.UtcNow.ToString(),
                                                STORE_ID = id_sitemap,
                                                stock = ScrapingDate.GetStock(HtmlUrl1, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                                                priceTag = ScrapingDate.GetPrice(HtmlUrl1, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                                                shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl1, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                                            };
                                            await App.PriceTrackerService.AddHistoryAsync(ProductHistory);
                                            await GetProductsAsync(OrderBy, OrderDescen);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
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
                        await GetProductsAsync(OrderBy, OrderDescen);
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
                List<ProductInfo> ProductsList = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                IsBusy = true;
                if (ProductsList != null && ProductsList.Count != 0)
                {
                    HideMessageFirstProduct();
                    OrderBy = order;
                    if (order == "name" && Ascendant == false)
                    {
                        OrderedList = ProductsList.OrderByDescending(o => o.productName).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "name" && Ascendant == true)
                    {
                        OrderedList = ProductsList.OrderBy(o => o.productName).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "id" && Ascendant == false)
                    {
                        OrderedList = ProductsList.OrderByDescending(o => o.ID_PRODUCT).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "id" && Ascendant == true)
                    {
                        OrderedList = ProductsList.OrderBy(o => o.ID_PRODUCT).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "price" && Ascendant == false)
                    {
                        OrderedList = ProductsList.OrderByDescending(o => o.PriceTag).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "price" && Ascendant == true)
                    {
                        OrderedList = ProductsList.OrderBy(o => o.PriceTag).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "stock" && Ascendant == false)
                    {
                        OrderedList = ProductsList.OrderByDescending(o => o.stock).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    else if (order == "stock" && Ascendant == true)
                    {
                        OrderedList = ProductsList.OrderBy(o => o.stock).ToList();
                        foreach (var item in OrderedList)
                        {
                            ListViewCollection.Add(item);
                        }
                    }
                    IsBusy = false;
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
