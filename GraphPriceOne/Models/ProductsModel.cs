using Microsoft.UI.Xaml;

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
        public bool IsCheckBoxChecked
        {
            get { return GetValue(() => IsCheckBoxChecked); }
            set { SetValue(() => IsCheckBoxChecked, value); }
        }
        public bool IsBusy
        {
            get { return GetValue(() => IsBusy); }
            set { SetValue(() => IsBusy, value); }
        }
        public bool SelectMultipleIsEnabled
        {
            get { return GetValue(() => SelectMultipleIsEnabled); }
            set { SetValue(() => SelectMultipleIsEnabled, value); }
        }
        //product info
        public int ID_PRODUCT
        {
            get { return GetValue(() => ID_PRODUCT); }
            set { SetValue(() => ID_PRODUCT, value); }
        }
        public string productName
        {
            get { return GetValue(() => productName); }
            set { SetValue(() => productName, value); }
        }
        public string ImageLocation
        {
            get { return GetValue(() => ImageLocation); }
            set { SetValue(() => ImageLocation, value); }
        }
        public string productDescription
        {
            get { return GetValue(() => productDescription); }
            set { SetValue(() => productDescription, value); }
        }
        public string productUrl
        {
            get { return GetValue(() => productUrl); }
            set { SetValue(() => productUrl, value); }
        }
        public double? PriceTag
        {
            get { return GetValue(() => PriceTag); }
            set { SetValue(() => PriceTag, value); }
        }
        public string priceCurrency
        {
            get { return GetValue(() => priceCurrency); }
            set { SetValue(() => priceCurrency, value); }
        }
        public string productDate
        {
            get { return GetValue(() => productDate); }
            set { SetValue(() => productDate, value); }
        }
        public string shippingPrice
        {
            get { return GetValue(() => shippingPrice); }
            set { SetValue(() => shippingPrice, value); }
        }

        public string shippingCurrency
        {
            get { return GetValue(() => shippingCurrency); }
            set { SetValue(() => shippingCurrency, value); }
        }
        public string storeName
        {
            get { return GetValue(() => storeName); }
            set { SetValue(() => storeName, value); }
        }
        public string stock
        {
            get { return GetValue(() => stock); }
            set { SetValue(() => stock, value); }
        }
    }
}
