using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.Models
{
    public class AddStoreModel : PropertyChangedNotification
    {
        public string StoreTittle
        {
            get { return GetValue(() => StoreTittle); }
            set { SetValue(() => StoreTittle, value); }
        }
        public BitmapImage Imagen
        {
            get { return GetValue(() => Imagen); }
            set { SetValue(() => Imagen, value); }
        }
        public string nameStore
        {
            get { return GetValue(() => nameStore); }
            set { SetValue(() => nameStore, value); }
        }
        public string startUrl
        {
            get { return GetValue(() => startUrl); }
            set { SetValue(() => startUrl, value); }
        }
        public string navbarLogo
        {
            get { return GetValue(() => navbarLogo); }
            set { SetValue(() => navbarLogo, value); }
        }
    }
}
