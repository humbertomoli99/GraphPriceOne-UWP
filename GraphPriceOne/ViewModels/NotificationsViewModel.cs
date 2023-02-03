using CommunityToolkit.Mvvm.Input;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.ViewModels
{
    public class NotificationsViewModel : NotificationsModel
    {

        public ObservableCollection<NotificationsModel> ListViewCollection { get; set; }

        public NotificationsViewModel()
        {
            ListViewCollection = new ObservableCollection<NotificationsModel>();
            _ = GetNotificationsAsync();

        }
        public ICommand RemoveItemCommand => new RelayCommand<int>(new Action<int>(async e => await RemoveItem(e)));
        public ICommand BuyNowCommand => new RelayCommand<string>(new Action<string>(async e => await BuyNow(e)));

        private async Task BuyNow(string Url_Product)
        {
            //DOCUMENTATION https://docs.microsoft.com/en-us/windows/uwp/launch-resume/launch-default-app
            var success = await Launcher.LaunchUriAsync(new Uri(Url_Product));

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }
        private async Task ExceptionDialog(string ex)
        {
            ContentDialog ExcepcionMessage = new ContentDialog()
            {
                Title = "Exception",
                PrimaryButtonText = "Ok",
                Content = ex.ToString()
            };
            await ExcepcionMessage.ShowAsync();
        }
        private async Task RemoveItem(int id_item)
        {
            await App.PriceTrackerService.DeleteNotificationAsync(id_item);
            await GetNotificationsAsync();
        }
        private async Task GetNotificationsAsync()
        {
            try
            {
                // Obtener la ruta de la carpeta local
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                // Obtener la lista de notificaciones
                var NotificationsList = (List<Notifications>)await App.PriceTrackerService.GetNotificationsAsync();

                // Inicializar la lista ordenada y la colección de ListView
                List<Notifications> OrderedList = new List<Notifications>();
                ListViewCollection.Clear();
                OrderedList.Clear();

                // Ordenar la lista de notificaciones por ID de notificación en orden descendente
                OrderedList = NotificationsList.OrderByDescending(o => o.ID_Notification).ToList();

                // Iterar a través de cada notificación en la lista ordenada
                foreach (var item in OrderedList)
                {
                    // Obtener la lista de productos y las imágenes de producto
                    List<ProductInfo> Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();

                    if (Products != null && Products.Any())
                    { break; }

                    var Product = Products.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID)).ToList();
                    List<ProductPhotos> Images = (List<ProductPhotos>)await App.PriceTrackerService.GetImagesAsync();
                    var ProductImages = Images.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID)).ToList();

                    // Crear el mensaje de precio actualizado
                    var drop = (item.NewPrice < item.PreviousPrice) ? "📉 Dropped" : "📈 Increased";
                    var message = drop + " (" + item.PreviousPrice + " to " + item.NewPrice + ")";

                    // Establecer la ubicación de la imagen
                    ImageLocation = "";
                    if (ProductImages != null && ProductImages.Count != 0)
                    {
                        ImageLocation = LocalState + ProductImages.First().PhotoSrc;
                    }

                    // Agregar la notificación a la colección de ListView
                    ListViewCollection.Add(new NotificationsModel()
                    {
                        PRODUCT_ID = item.PRODUCT_ID,
                        ID_Notification = item.ID_Notification,
                        ProductName = Product.FirstOrDefault().productName,
                        ProductDescription = message,
                        NewPrice = item.NewPrice,
                        PreviousPrice = item.PreviousPrice,
                        ProductUrl = Product.FirstOrDefault().productUrl,
                        ImageLocation = ImageLocation
                    });

                }
            }
            catch (Exception ex)
            {
                // Mostrar un diálogo de excepción
                await ExceptionDialog(ex.ToString());
            }
        }


    }
}
