using CommunityToolkit.Mvvm.Input;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Library;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Microsoft.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.ViewModels
{


    public class AddSelectorsViewModel : AddSelectorsModel
    {
        private UploadImage _uploadImage;
        private BitmapImage _bitmapImage;
        public StorageFile imagePath;
        private string _imageSrc;
        private byte[] avatar;
        public AddSelectorsViewModel()
        {
            _uploadImage = new UploadImage();
            _bitmapImage = new BitmapImage();

            IconBitmap = _bitmapImage;
            _bitmapImage = new BitmapImage();
            if (SelectedStore != null)
            {
                _ = GetSelectorsAsync();
                //IconSvg = STORESELECT.IconSvg;
                IconBitmap = SelectedStore.IconBitmap;
                nameStore = SelectedStore.nameStore;
                startUrl = SelectedStore.startUrl;
            }

        }
        public ICommand ICommandExaminar => new RelayCommand(new Action(async () => await ChangeImageAsync()));
        public ICommand ICommandCancel => new RelayCommand(new Action(() => NavigationService.Navigate(typeof(StoresPage))));
        public ICommand ICommandAddSelectors => new RelayCommand(new Action(async () => await AddSelectorsAsync()));
        private async Task ChangeImageAsync()
        {
            object[] objects = await _uploadImage.LoadImageAsync();
            avatar = (byte[])objects[0];//va a la base de datos
            if (avatar != null)
            {
                _bitmapImage = (BitmapImage)objects[1];
                IconBitmap = _bitmapImage;
                imagePath = (StorageFile)objects[2];
            }
        }
        public async Task AddSelectorsAsync()
        {
            if (imagePath != null)
            {
                _imageSrc = await UploadImage.SaveImageAsync(imagePath, @"\Stores\", nameStore);
            }
            Selector SelectorObjects = new Selector()
            {
                ID_SELECTOR = SelectedStore.ID_STORE,
                Title = Title,
                TitleGetAttribute = TitleGetAttribute,
                TitleNotNull = TitleNotNull ? 1 : 0,
                Description = Description,
                DescriptionGetAttribute = DescriptionGetAttribute,
                DescriptionNotNull = DescriptionNotNull ? 1 : 0,
                Price = Price,
                PriceGetAttribute = PriceGetAttribute,
                PriceNotNull = PriceNotNull ? 1 : 0,
                Images = Images,
                ImagesNotNull = ImagesNotNull ? 1 : 0,
                CurrencyPrice = CurrencyPrice,
                CurrencyPriceGetAttribute = CurrencyPriceGetAttribute,
                PriceCurrencyNotNull = CurrencyPriceNotNull ? 1 : 0,
                Shipping = Shipping,
                ShippingGetAttribute = ShippingGetAttribute,
                ShippingNotNull = ShippingNotNull ? 1 : 0,
                ShippingCurrency = ShippingCurrency,
                ShippingCurrencyGetAttribute = ShippingCurrencyGetAttribute,
                ShippingCurrencyNotNull = ShippingCurrencyNotNull ? 1 : 0,
                Stock = Stock,
                StockGetAttribute = StockGetAttribute,
                StockNotNull = StockNotNull ? 1 : 0,
            };
            Store StoreObjects = new Store()
            {
                ID_STORE = SelectedStore.ID_STORE,
                nameStore = SelectedStore.nameStore,
                startUrl = SelectedStore.startUrl,
                image = _imageSrc
            };
            await App.PriceTrackerService.UpdateStoreAsync(StoreObjects);
            await App.PriceTrackerService.UpdateSelectorAsync(SelectorObjects);
            NavigationService.Navigate(typeof(StoresPage));
        }
        public async Task GetSelectorsAsync()
        {
            //var ListSelectors = new List<AddSelectorsModel>();
            var Selectors = await App.PriceTrackerService.GetSelectorsAsync();
            var lista = Selectors.Where(u => u.ID_SELECTOR.Equals(SelectedStore.ID_STORE)).ToList();
            //var lista = _sqlite.Connection.Table<Selector>().Where(u => u.ID_SELECTOR.Equals(STORESELECT.ID_STORE)).ToList();

            if (lista.Count == 0)
            {
                Selector SelectorObjects = new Selector();
                SelectorObjects.ID_SELECTOR = SelectedStore.ID_STORE;
                await App.PriceTrackerService.AddSelectorAsync(SelectorObjects);
                await AddSelectorsAsync();
            }
            else
            {
                var Select = lista.First();

                Title = Select.Title;
                TitleNotNull = (Select.TitleNotNull == 1) ? true : false;
                TitleGetAttribute = Select.TitleGetAttribute;

                Description = Select.Description;
                DescriptionNotNull = (Select.DescriptionNotNull == 1) ? true : false;
                DescriptionGetAttribute = Select.DescriptionGetAttribute;

                Price = Select.Price;
                PriceNotNull = (Select.PriceNotNull == 1) ? true : false;
                PriceGetAttribute = Select.PriceGetAttribute;

                Images = Select.Images;
                ImagesNotNull = (Select.ImagesNotNull == 1) ? true : false;

                CurrencyPrice = Select.CurrencyPrice;
                CurrencyPriceNotNull = (Select.PriceCurrencyNotNull == 1) ? true : false;
                CurrencyPriceGetAttribute = Select.CurrencyPriceGetAttribute;

                Shipping = Select.Shipping;
                ShippingNotNull = (Select.ShippingNotNull == 1) ? true : false;
                ShippingGetAttribute = Select.ShippingGetAttribute;

                ShippingCurrency = Select.ShippingCurrency;
                ShippingCurrencyNotNull = (Select.ShippingCurrencyNotNull == 1) ? true : false;
                ShippingCurrencyGetAttribute = Select.ShippingCurrencyGetAttribute;

                Stock = Select.Stock;
                StockNotNull = (Select.StockNotNull == 1) ? true : false;
                StockGetAttribute = Select.StockGetAttribute;
            }
        }
    }
}
