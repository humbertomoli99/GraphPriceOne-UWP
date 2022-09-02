using CommunityToolkit.Mvvm.Input;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.ViewModels
{
    public class StoresViewModel : StoresModel
    {
        private ListView _ListView;
        private bool SelectMultipleIsEnabled;
        public StoresViewModel()
        {
        }
        public StoresViewModel(object[] campos)
        {
            _ListView = (ListView)campos[0];
            _ = GetStoresAsync();
            HideButtons();
        }
        public ICommand GoToViewAddStore => new RelayCommand(new Action(() => NavigationService.Navigate(typeof(AddStorePage))));
        public ICommand SelectMultiple => new RelayCommand(new Action(() => SelectMulti()));
        //public ICommand CheckedAll => new RelayCommand(new Action(async () => await SelectAll()));
        public ICommand UpdateList => new RelayCommand(new Action(async () => await GetStoresAsync()));
        public ICommand DeleteStore => new RelayCommand(new Action(async () => await DeleteAsync()));
        private void HideButtons()
        {
            _ListView.SelectedItem = null;
            SelectMultipleIsEnabled = false;
            _ListView.SelectionMode = ListViewSelectionMode.Single;
            isCheckedAllVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            DeleteStoreVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        private void ShowButtons()
        {
            SelectMultipleIsEnabled = true;
            _ListView.SelectionMode = ListViewSelectionMode.Multiple;
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
                //_ListView.IsMultiSelectCheckBoxEnabled = true;
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
        private async Task DeleteAsync()
        {
            try
            {
                IList<object> itemsSelected = _ListView.SelectedItems;
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
                            StoresModel data = (StoresModel)item;
                            int data1 = data.ID_STORE;
                            string NameImage = data?.image?.ToString();
                            if (NameImage != null)
                            {
                                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                                string FolderItemDelete = Path.Combine(LocalState, "Stores");
                                string path = Path.Combine(FolderItemDelete, NameImage);
                                File.Delete(path);
                            }
                            await App.PriceTrackerService.DeleteStoreAsync(data1);
                            await App.PriceTrackerService.DeleteSelectorAsync(data1);

                            //_sqlite.Connection.Table<STORE>().Where(u => u.ID_STORE.Equals(data1)).Delete();
                            //_sqlite.Connection.Table<SELECTOR>().Where(u => u.ID_SELECTOR.Equals(data1)).Delete();
                        }
                        await GetStoresAsync();
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
        public async Task GetStoresAsync()
        {
            var ListProduct = new List<StoresModel>();
            List<Store> lista = (List<Store>)await App.PriceTrackerService.GetStoresAsync();

            if (lista != null)
            {
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                foreach (var item in lista)
                {
                    var bitmapImage = new BitmapImage();
                    var bitmapFolder = Path.Combine(LocalState, @"Stores\");
                    string bitmapUri = bitmapFolder + item?.image?.ToString();
                    bitmapImage.UriSource = new Uri(bitmapUri);
                    string format = Path.GetExtension(bitmapUri);
                    if (format == ".svg")
                    {
                        //sacamos el alto y ancho real de la imagen svg
                        //SVGDocument document = new Aspose.Svg.SVGDocument(bitmapUri);
                        //double width = document.RootElement.Width.AnimVal.Value;
                        //double height = document.RootElement.Height.AnimVal.Value;

                        //abrimos y le asignamos el tamaño real de la imagen svg para que no aparezca recortada o muy pequeña
                        //var svgSource = new SvgImageSource(bitmapImage.UriSource)
                        //{
                        //    RasterizePixelHeight = height,
                        //    RasterizePixelWidth = width,
                        //};

                        ListProduct.Add(new StoresModel
                        {
                            ID_STORE = item.ID_STORE,
                            nameStore = item.nameStore,
                            image = item.image,
                            startUrl = item.startUrl,
                            //IconSvg = svgSource,
                        });
                    }
                    else
                    {
                        //var image = await _uploadImage.ImageFromBufferAsync(item.Images);
                        ListProduct.Add(new StoresModel
                        {
                            ID_STORE = item.ID_STORE,
                            nameStore = item.nameStore,
                            image = item.image,
                            startUrl = item.startUrl,
                            IconBitmap = bitmapImage,
                        });
                    }
                }
                ListStoresVM = ListProduct;
            }
        }
    }
}
