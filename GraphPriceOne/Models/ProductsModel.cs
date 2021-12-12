using Windows.UI.Xaml;

namespace GraphPriceOne.Models
{
    public class ProductsModel : PropertyChangedNotification
    {
        public Visibility IsCheckedAllVisibility
        {
            get { return GetValue(() => IsCheckedAllVisibility); }
            set { SetValue(() => IsCheckedAllVisibility, value); }
        }
        public Visibility DeleteCommandVisibility
        {
            get { return GetValue(() => DeleteCommandVisibility); }
            set { SetValue(() => DeleteCommandVisibility, value); }
        }
        public Visibility FirstProductVisibility
        {
            get { return GetValue(() => FirstProductVisibility); }
            set { SetValue(() => FirstProductVisibility, value); }
        }
        public Visibility CommandBarVisibility
        {
            get { return GetValue(() => CommandBarVisibility); }
            set { SetValue(() => CommandBarVisibility, value); }
        }
        public Visibility ListProductsVisibility
        {
            get { return GetValue(() => ListProductsVisibility); }
            set { SetValue(() => ListProductsVisibility, value); }
        }
    }
}
