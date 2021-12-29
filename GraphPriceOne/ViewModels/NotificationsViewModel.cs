using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            //var Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();

            var NotificationsList = (List<Notifications>)await App.PriceTrackerService.GetNotificationsAsync();
            //List<NotificationsModel> ListaExit = new List<NotificationsModel>();
            ListViewCollection.Clear();
            foreach (var item in NotificationsList)
            {

                //var Product = (ProductInfo)Products.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID));

                //var Images = await App.PriceTrackerService.GetImagesAsync();
                //var ProductImage = Images.Where(u => u.ID_PRODUCT.Equals(Product.ID_PRODUCT)).FirstOrDefault();

                //string mensaje = item.Message?.Replace("\n", "");
                var message = "📉 Dropped " + item.PRODUCT_ID + " (" + item.PreviousPrice + " to " + item.NewPrice + ")";
                ListViewCollection.Add(new NotificationsModel()
                {
                    PRODUCT_ID = item.PRODUCT_ID,
                    ID_Notification = item.ID_Notification,
                    ProductName = item.PRODUCT_ID.ToString(),
                    ProductDescription = message,
                    NewPrice = item.NewPrice,
                    PreviousPrice = item.PreviousPrice,
                    ProductUrl = "https://www.youtube.com/",
                    ImageLocation = ""
                });
            }
        }
    }
}
