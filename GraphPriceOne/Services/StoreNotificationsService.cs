using GraphPriceOne.Activation;
using GraphPriceOne.ViewModels;
using GraphPriceOne.Views;
using Microsoft.Services.Store.Engagement;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;

namespace GraphPriceOne.Services
{
    internal class StoreNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        private ProductDetailsViewModel selectors;
        public async Task InitializeAsync()
        {
            try
            {
                selectors = new ProductDetailsViewModel();
                var engagementManager = StoreServicesEngagementManager.GetDefault();
                await engagementManager.RegisterNotificationChannelAsync();
            }
            catch (Exception)
            {
                // TODO WTS: Channel registration call can fail, please handle exceptions as appropriate to your scenario.
            }
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

            var engagementManager = StoreServicesEngagementManager.GetDefault();
            string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);

            //// Use the originalArgs variable to access the original arguments passed to the app.
            ///

            // Obtain the arguments from the notification
            ToastArguments argumentsParsed = ToastArguments.Parse(toastActivationArgs.Argument);

            // Obtain any user input (text boxes, menu selections) from the notification
            ValueSet userInput = toastActivationArgs.UserInput;

            int ProductId;
            if (argumentsParsed["Action"] == "viewProduct")
            {
                ProductId = Int32.Parse(argumentsParsed["ProductId"]);
                selectors.SelectedProduct = ProductId;
                NavigationService.Navigate(typeof(ProductDetailsPage));
            }

            await Task.CompletedTask;
        }
    }
}
