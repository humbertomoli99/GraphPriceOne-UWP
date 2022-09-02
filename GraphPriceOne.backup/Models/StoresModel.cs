using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.Models
{
    public class StoresModel : PropertyChangedNotification
    {
        public int ID_STORE
        {
            get { return GetValue(() => ID_STORE); }
            set { SetValue(() => ID_STORE, value); }
        }
        public BitmapImage IconBitmap
        {
            get { return GetValue(() => IconBitmap); }
            set { SetValue(() => IconBitmap, value); }
        }
        //public SvgImageSource IconSvg
        //{
        //    get { return GetValue(() => IconSvg); }
        //    set { SetValue(() => IconSvg, value); }
        //}
        public string image
        {
            get { return GetValue(() => image); }
            set { SetValue(() => image, value); }
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
        public string StoreTittle
        {
            get { return GetValue(() => StoreTittle); }
            set { SetValue(() => StoreTittle, value); }
        }
        public Visibility isCheckedAllVisibility
        {
            get { return GetValue(() => isCheckedAllVisibility); }
            set { SetValue(() => isCheckedAllVisibility, value); }
        }
        public Visibility DeleteStoreVisibility
        {
            get { return GetValue(() => DeleteStoreVisibility); }
            set { SetValue(() => DeleteStoreVisibility, value); }
        }
        public bool? isCheckedAll
        {
            get { return GetValue(() => isCheckedAll); }
            set { SetValue(() => isCheckedAll, value); }
        }
        public string isIndeterminate
        {
            get { return GetValue(() => isIndeterminate); }
            set { SetValue(() => isIndeterminate, value); }
        }
        public List<StoresModel> ListStoresVM
        {
            get { return GetValue(() => ListStoresVM); }
            set { SetValue(() => ListStoresVM, value); }
        }
    }
}
