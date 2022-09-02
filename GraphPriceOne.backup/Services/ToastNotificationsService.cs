using GraphPriceOne.Activation;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Library;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace GraphPriceOne.Services
{
    internal partial class ToastNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public async Task ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
                // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            //// TODO WTS: Handle activation from toast notification
            //// More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }
        public static async void ShowToastNotification(string title, string stringContent, ProductInfo PRODUCT)
        {
            try
            {
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                var Images = await App.PriceTrackerService.GetImagesAsync();
                var ProductImage = Images.Where(u => u.ID_PRODUCT.Equals(PRODUCT.ID_PRODUCT)).ToList().FirstOrDefault();

                new ToastContentBuilder()
                        .AddHeroImage(new Uri(LocalState + ProductImage.PhotoSrc))
                        .AddArgument("Action", "viewProduct")
                        .AddArgument("ProductId", PRODUCT.ID_PRODUCT)
                        .AddText(title)
                        .AddText(stringContent)

                        //Buttons
                        .AddButton(new ToastButton()
                        .SetContent("Buy now")
                        .AddArgument("action", "buy")
                        .SetBackgroundActivation())


                        .AddButton(new ToastButton()
                        .SetContent("Stop following")
                        .AddArgument("action", "stopfollow")
                        .SetBackgroundActivation())

                        .Show(toast =>
                        {
                            toast.ExpirationTime = DateTime.Now.AddDays(1);
                        }
                        );
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
            }
        }
    }
}
