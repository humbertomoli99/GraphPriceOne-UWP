using CommunityToolkit.Mvvm.Input;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Library;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.Views;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.ViewModels
{
    public class AddStoreViewModel : AddStoreModel
    {
        private TextBox _StoreName;
        private TextBox _StoreURL;
        private UploadImage _uploadImage;
        private BitmapImage _bitmapImage;
        private string _imageSrc;
        private byte[] avatar;
        private StorageFile imagePath;
        public AddStoreViewModel(object[] campos)
        {
            _StoreName = (TextBox)campos[0];
            _StoreURL = (TextBox)campos[1];
            _uploadImage = new UploadImage();
            _bitmapImage = new BitmapImage();
            ResetStores();
        }
        public ICommand loadImage => new RelayCommand(new Action(async () => await LoadImageAsync()));
        public ICommand ICommandAddStore => new RelayCommand(new Action(async () => await AddStoreAsync()));
        public ICommand ICommandCancel => new RelayCommand(new Action(() => NavigationService.Navigate(typeof(StoresPage))));
        private async Task AddStoreAsync()
        {
            try
            {
                if (nameStore == null || nameStore.Equals(""))
                {
                    StoreTittle = "Ingrese un Nombre para el Sitemap";
                    _StoreName.Focus(FocusState.Programmatic);
                }
                else
                {
                    if (startUrl == null || startUrl.Equals(""))
                    {
                        StoreTittle = "Ingrese una URL para el Sitemap";
                        _StoreURL.Focus(FocusState.Programmatic);
                    }
                    else
                    {
                        if (!TextBoxEvent.IsValidURL(startUrl))
                        {
                            StoreTittle = "Ingrese una URL valida";
                            _StoreURL.Focus(FocusState.Programmatic);
                        }
                        else
                        {
                            List<Store> Stores = (List<Store>)await App.PriceTrackerService.GetStoresAsync();

                            var query = Stores.Where(s => s.nameStore.Equals(nameStore)).ToList();
                            if (0 < query.Count)
                            {
                                StoreTittle = "Ingrese un Sitemap que no este registrado";
                                _StoreName.Focus(FocusState.Programmatic);
                            }
                            else
                            {
                                var query3 = Stores.Where(s => s.startUrl.Equals(startUrl)).ToList();
                                if (0 < query3.Count)
                                {
                                    StoreTittle = "Ingrese una url que no este registrada";
                                    _StoreURL.Focus(FocusState.Programmatic);
                                }
                                else
                                {
                                    await SaveDataStoreAsync();
                                    App.mContentFrame.Navigate(typeof(StoresPage));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private async Task SaveDataStoreAsync()
        {
            HtmlNode HtmlPage = await ScrapingDate.LoadPageAsync(startUrl);

            //if (navbarLogo != string.Empty)
            //{
            //    string navbarSrc = ScrapingDate.GetUrlImage(HtmlPage, navbarLogo);
            //    image = ScrapingDate.DownloadImage(startUrl, navbarSrc, @"\Stores\", nameStore);
            //}
            //else
            //{
            ScrapingDate.EnlaceImage MetaIcon = ScrapingDate.GetMetaIcon(HtmlPage);
            if (imagePath != null)
            {
                _imageSrc = await UploadImage.SaveImageAsync(imagePath, @"\Stores\", nameStore);
            }
            else
            {
                _imageSrc = ScrapingDate.DownloadMetaIcon(startUrl, MetaIcon, @"\Stores\", nameStore).ToString();
            }
            //}

            Store StoreObjects = new Store()
            {
                nameStore = nameStore,
                startUrl = startUrl,
                image = _imageSrc
            };

            await App.PriceTrackerService.AddStoreAsync(StoreObjects);
        }
        private async Task LoadImageAsync()
        {
            object[] objects = await _uploadImage.LoadImageAsync();
            avatar = (byte[])objects[0];//va a la base de datos
            if (avatar != null)
            {
                _bitmapImage = (BitmapImage)objects[1];
                Imagen = _bitmapImage;
                imagePath = (StorageFile)objects[2];
            }
            else
            {
                if (0 == _bitmapImage.PixelHeight)
                {
                    _bitmapImage.UriSource = new Uri("ms-appx:///Assets/StoreLogo.scale-400.png");
                    avatar = await _uploadImage.ImagebyteAsync(_bitmapImage);
                    Imagen = _bitmapImage;
                }
            }
        }
        private void ResetStores()
        {
            _bitmapImage.UriSource = new Uri("ms-appx:///Assets/StoreLogo.scale-400.png");
            Imagen = _bitmapImage;
            _bitmapImage = new BitmapImage();
        }
    }
}
