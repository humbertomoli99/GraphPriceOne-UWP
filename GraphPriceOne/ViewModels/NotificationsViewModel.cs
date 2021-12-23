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
        public ICommand RemoveItemCommand => new RelayCommand<NotificationsModel>(new Action<NotificationsModel>(async e => await RemoveItem(e)));
        private async Task RemoveItem(NotificationsModel item)
        {
            await App.PriceTrackerService.DeleteNotificationAsync(item.ID_Notification);
            await GetNotificationsAsync();
        }

        private async Task GetNotificationsAsync()
        {
            List<Notifications> NotificationsList = (List<Notifications>)await App.PriceTrackerService.GetNotificationsAsync();
            //List<ProductInfo> Products = (List<ProductInfo>)await App.PriceTrackerService.GetProductsAsync();
            //List<NotificationsModel> ListaExit = new List<NotificationsModel>();
            ListViewCollection.Clear();
            foreach (var item in NotificationsList)
            {
                //ProductInfo Product = (ProductInfo)Products.Where(u => u.ID_PRODUCT.Equals(item.PRODUCT_ID));

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
                    //ProductUrl = Product.productUrl
                });
            }
        }
    }
}
