using System;
using System.Threading.Tasks;

using GraphPriceOne.Activation;
using GraphPriceOne.Library;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
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
        public static async void ShowToastNotification(string title, string stringContent)
        {
            try
            {
                ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
                Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
                toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
                Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
                audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

                ToastNotification toast = new ToastNotification(toastXml);
                //toast.ExpirationTime = DateTime.Now.AddSeconds(4);
                ToastNotifier.Show(toast);
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
            }
        }
    }
}
