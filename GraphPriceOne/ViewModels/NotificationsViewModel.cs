using GraphPriceOne.Core.Models;
using GraphPriceOne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.ViewModels
{
    public class NotificationsViewModel : NotificationsModel
    {
        public ObservableCollection<NotificationsModel> ListViewCollection { get; set; }

        public NotificationsViewModel(ListView ListViewControl)
        {
            ListViewCollection = new ObservableCollection<NotificationsModel>();
            _ = GetNotificationsAsync();

        }
        private async Task GetNotificationsAsync()
        {
            List<Notifications> NotificationsList = (List<Notifications>)await App.PriceTrackerService.GetNotificationsAsync();

            foreach (var item in NotificationsList)
            {
                //var PRODUCT_ID = item.PRODUCT_ID != null ? item.PRODUCT_ID : 1;
                //ProductInfo itemNotify = await App.PriceTrackerService?.GetProductAsync(PRODUCT_ID);
                //string mensaje = item.Message?.Replace("\n", "");
                var message = "📉 Dropped \n" + item.PRODUCT_ID + "\n (" + item.PreviousPrice + " to " + item.NewPrice + ")";
                ListViewCollection.Add(new NotificationsModel()
                {
                    ID_PRODUCT = item.PRODUCT_ID,
                    ProductName = item.PRODUCT_ID.ToString(),
                    ProductDescription = message,
                    NewPrice = item.NewPrice,
                    PreviousPrice = item.PreviousPrice
                });
            }
        }
    }
}
