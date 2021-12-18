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
        public ObservableCollection<ProductsModel> ListViewCollection { get; set; }
        public ProductPhotos ProductImages { get; private set; }
        private List<ProductInfo> OrderedList { get; set; }

        public History ProductHistory { get; private set; }
        public string OrderBy;
        public bool OrderDescen;

        public MainViewModel(ListView ListViewControl)
        {
            IsBusy = false;

            ListViewCollection = new ObservableCollection<ProductsModel>();
            _ = GetProductsAsync();

            ShowMessageFirstProduct();

            this.ListViewControl = ListViewControl;
        }
        public MainViewModel()
        {

        }
        public ICommand SelectMultipleCommand => new RelayCommand(new Action(() => SelectMulti()));
        public ICommand ClearFilterCommand => new RelayCommand(new Action(async () => await GetProductsAsync()));
        public ICommand OrderDescendentCommand => new RelayCommand(new Action(async () => await ShowOrderedList(OrderBy, false)));
        public ICommand OrderAscendantCommand => new RelayCommand(new Action(async () => await ShowOrderedList(OrderBy, true)));
        public ICommand OrderByNameCommand => new RelayCommand(new Action(async () => await ShowOrderedList("name", OrderDescen)));
        public ICommand OrderByPriceCommand => new RelayCommand(new Action(async () => await ShowOrderedList("price", OrderDescen)));
        public ICommand OrderByStockCommand => new RelayCommand(new Action(async () => await ShowOrderedList("stock", OrderDescen)));
        public ICommand AddProductCommand => new RelayCommand(new Action(async () => await AddProductAsync()));
        public ICommand UpdateListCommand => new RelayCommand(new Action(async () => await ShowOrderedList(OrderBy, OrderDescen)));
        public ICommand DeleteCommand => new RelayCommand(new Action(async () => await DeleteAsync()));
        private async Task AddProductAsync()
        {
            IsBusy = true;
            try
            {
                string url = await ClipboardEvents.OutputClipboardTextAsync();

                List<ProductInfo> Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                var query = Products.Where(s => s.productUrl.Equals(url))?.ToList();

                bool IsRegistered = ((Products.Where(s => s.productUrl.Equals(url))?.ToList().Count) > 0) ? true : false;

                var Stores = await App.PriceTrackerService.GetStoresAsync();
                var UrlShop = Stores.Where(s => url.Contains(s.startUrl))?.ToList();

                //url no valida
                //validar url valida para envio masivo de productos
                if (!TextBoxEvent.IsValidURL(url))
                {
                    ContentDialog InvalidClipboardUrl = new ContentDialog()
                    {
                        Title = "Add a new product",
                        PrimaryButtonText = "OK, THANKS",
                        Content = "Copy a URL to begin\n" + "Copy the URL of the product, then select Add Product to start tracking the product's price."
                    };
                    await InvalidClipboardUrl.ShowAsync();
                    return;
                }
                if (UrlShop.Count == 0 || UrlShop == null)
                {
                    ContentDialog UnassignedSectors = new ContentDialog()
                    {
                        Title = "No selectors assigned to Store",
                        PrimaryButtonText = "OK",
                        SecondaryButtonText = "MASS SHIPPING",
                        CloseButtonText = "CANCEL",
                        Content = "The store has no assigned sectors."
                    };
                    ContentDialogResult result1 = await UnassignedSectors.ShowAsync();
                    if (result1 == ContentDialogResult.Primary)
                    {
                        NavigationService.Navigate(typeof(AddStorePage));
                    }
                    else if (result1 == ContentDialogResult.Secondary)
                    {
                        // obtener todas las url de la pagina, y pasarla por el filtro si existe la tienda con selectores
                        await MassShipping(url);
                    }
                    return;
                }
                //crear un if por si el sitemap no tiene selectores
                if (IsRegistered)
                {
                    ContentDialog ProductRegisterMessage = new ContentDialog()
                    {
                        Title = "This product is already registered",
                        PrimaryButtonText = "OK",
                        Content = "The product is registered and will continue to be tracked."
                    };
                    await ProductRegisterMessage.ShowAsync();
                    return;
                }
                HtmlNode HtmlUrl = await ScrapingDate.LoadPageAsync(url);
                ScrapingDate.EnlaceImage icon = ScrapingDate.GetMetaIcon(HtmlUrl);

                var id_sitemap = UrlShop.First().ID_STORE;
                var Selectores = await App.PriceTrackerService.GetSelectorsAsync();

                var SitemapSelectors = Selectores.Where(s => s.ID_SELECTOR.Equals(id_sitemap))?.ToList()?.First();
                ProductInfo Product = new ProductInfo()
                {
                    ID_STORE = id_sitemap,
                    productName = ScrapingDate.GetTitle(HtmlUrl, SitemapSelectors.Title),
                    productUrl = url,
                    productDescription = ScrapingDate.GetDescription(HtmlUrl, SitemapSelectors.Description, SitemapSelectors.DescriptionGetAttribute),
                    stock = ScrapingDate.GetStock(HtmlUrl, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                    PriceTag = ScrapingDate.GetPrice(HtmlUrl, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
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

                    List<ProductInfo> Products2 = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                    var lastId = Products2[Products2.Count - 1].ID_PRODUCT;

                    // si hay 0 items es 1;
                    //for para añadir todas las imagenes encontradas
                    List<string> imagen = ScrapingDate.GetUrlImage(HtmlUrl, SitemapSelectors.Images);

                    string[] imagenes = ScrapingDate.DownloadImage(url, imagen, @"\Products\", lastId.ToString());
                    if (imagenes != null)
                    {
                        foreach (var item in imagenes)
                        {
                            ProductPhotos ProductImages = new ProductPhotos()
                            {
                                PhotoSrc = item,
                                ID_PRODUCT = lastId,

                            };
                            await App.PriceTrackerService.AddImageAsync(ProductImages);
                        }
                    }

                    History ProductHistory = new History()
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
                    await GetProductsAsync();
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    HideMessageFirstProduct();
                    await MassShipping(url);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ExceptionDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
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
            await GetProductsAsync();
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

            ProductInfo Product = new ProductInfo()
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

                                            List<ProductInfo> Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                                            var lastId = (Products.Count == 0) ? 1 : Products[Products.Count - 1].ID_PRODUCT;

                                            //for para añadir todas las imagenes encontradas
                                            List<string> imagen = ScrapingDate.GetUrlImage(HtmlUrl1, SitemapSelectors.Images);

                                            string[] imagenes = ScrapingDate.DownloadImage(url, imagen, @"\Products\", lastId.ToString());
                                            if (imagenes != null)
                                            {
                                                foreach (var item in imagenes)
                                                {
                                                    ProductPhotos ProductImages = new ProductPhotos()
                                                    {
                                                        PhotoSrc = item,
                                                        ID_PRODUCT = lastId,
                                                    };
                                                    await App.PriceTrackerService.AddImageAsync(ProductImages);
                                                }
                                            }

                                            History ProductHistory = new History()
                                            {
                                                PRODUCT_ID = lastId,
                                                productDate = DateTime.UtcNow.ToString(),
                                                STORE_ID = id_sitemap,
                                                stock = ScrapingDate.GetStock(HtmlUrl1, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                                                priceTag = ScrapingDate.GetPrice(HtmlUrl1, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                                                shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl1, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                                            };
                                            await App.PriceTrackerService.AddHistoryAsync(ProductHistory);
                                            await GetProductsAsync();
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
                            ProductsModel data = (ProductsModel)item;
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
                await ExceptionDialog(ex);
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
        public async Task GetProductsAsync()
        {
            try
            {
                IsBusy = true;
                List<ProductInfo> ProductsList = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                if (ProductsList != null && ProductsList.Count != 0)
                {
                    HideMessageFirstProduct();
                    await ShowOrderedList();
                }
                else
                {
                    ShowMessageFirstProduct();
                }
            }
            catch (Exception ex)
            {
                await ExceptionDialog(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task ShowOrderedList(string order = "id", bool Ascendant = false)
        {
            try
            {
                OrderBy = order;
                OrderDescen = Ascendant;
                ListViewCollection.Clear();
                List<ProductInfo> ProductsList = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
                if (order == "name" && Ascendant == false)
                {
                    OrderedList = ProductsList.OrderByDescending(o => o.productName).ToList();
                }
                else if (order == "name" && Ascendant == true)
                {
                    OrderedList = ProductsList.OrderBy(o => o.productName).ToList();
                }
                else if (order == "id" && Ascendant == false)
                {
                    OrderedList = ProductsList.OrderByDescending(o => o.ID_PRODUCT).ToList();
                }
                else if (order == "id" && Ascendant == true)
                {
                    OrderedList = ProductsList.OrderBy(o => o.ID_PRODUCT).ToList();
                }
                else if (order == "price" && Ascendant == false)
                {
                    OrderedList = ProductsList.OrderByDescending(o => o.PriceTag).ToList();
                }
                else if (order == "price" && Ascendant == true)
                {
                    OrderedList = ProductsList.OrderBy(o => o.PriceTag).ToList();
                }
                else if (order == "stock" && Ascendant == false)
                {
                    OrderedList = ProductsList.OrderByDescending(o => o.stock).ToList();
                }
                else if (order == "stock" && Ascendant == true)
                {
                    OrderedList = ProductsList.OrderBy(o => o.stock).ToList();
                }
                foreach (var item in OrderedList)
                {
                    string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                    List<ProductPhotos> Images = (List<ProductPhotos>)await App.PriceTrackerService.GetImagesAsync();
                    var ProductImages = Images.Where(Img => Img.ID_PRODUCT.Equals(item.ID_PRODUCT)).ToList();

                    List<History> Histories = (List<History>)await App.PriceTrackerService.GetHistoriesAsync();
                    var ProductHistory = Histories.Where(u => u.PRODUCT_ID.Equals(item.ID_PRODUCT)).ToList();

                    var LastHistory = ProductHistory.Count - 1;

                    ImageLocation = "";
                    if (ProductImages != null && ProductImages.Count != 0)
                    {
                        ImageLocation = LocalState + ProductImages.First().PhotoSrc;
                    }
                    //shipping currency
                    shippingCurrency = (shippingCurrency == null) ? "$" : shippingCurrency;
                    //shipping price
                    if (item.shippingPrice == 0)
                    {
                        shippingPrice = "Free Shipping";
                    }
                    else if (item.shippingPrice == null)
                    {
                        shippingPrice = "Not Available";
                    }
                    else
                    {
                        shippingPrice = shippingCurrency + ProductHistory[LastHistory].shippingPrice;
                    }
                    //get stock
                    if (item.stock == null)
                    {
                        stock = "Not Available";
                    }
                    else
                    {
                        stock = ProductHistory[LastHistory].stock.ToString();
                    }

                    ListViewCollection.Add(new ProductsModel()
                    {
                        ID_PRODUCT = item.ID_PRODUCT,
                        productName = item.productName,
                        productDescription = item.productDescription,
                        productUrl = item.productUrl,
                        PriceTag = ProductHistory[LastHistory].priceTag,
                        priceCurrency = item.priceCurrency,
                        shippingPrice = shippingPrice,
                        shippingCurrency = shippingCurrency,
                        stock = stock,
                        ImageLocation = ImageLocation
                    });
                }
            }
            catch (Exception ex)
            {
                await ExceptionDialog(ex);
            }
        }
        private async Task ExceptionDialog(Exception ex)
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
