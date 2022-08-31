namespace GraphPriceOne.Models
{
    public class NotificationsModel : PropertyChangedNotification
    {
        public bool IsBusy
        {
            get { return GetValue(() => IsBusy); }
            set { SetValue(() => IsBusy, value); }
        }
        public int ID_Notification
        {
            get { return GetValue(() => ID_Notification); }
            set { SetValue(() => ID_Notification, value); }
        }
        public int PRODUCT_ID
        {
            get { return GetValue(() => PRODUCT_ID); }
            set { SetValue(() => PRODUCT_ID, value); }
        }
        public string ProductName
        {
            get { return GetValue(() => ProductName); }
            set { SetValue(() => ProductName, value); }
        }
        public string ImageLocation
        {
            get { return GetValue(() => ImageLocation); }
            set { SetValue(() => ImageLocation, value); }
        }
        public string ProductDescription
        {
            get { return GetValue(() => ProductDescription); }
            set { SetValue(() => ProductDescription, value); }
        }
        public string ProductUrl
        {
            get { return GetValue(() => ProductUrl); }
            set { SetValue(() => ProductUrl, value); }
        }
        //previous price
        public double? PreviousPrice
        {
            get { return GetValue(() => PreviousPrice); }
            set { SetValue(() => PreviousPrice, value); }
        }
        public double? NewPrice
        {
            get { return GetValue(() => NewPrice); }
            set { SetValue(() => NewPrice, value); }
        }
        public double? PriceTag
        {
            get { return GetValue(() => PriceTag); }
            set { SetValue(() => PriceTag, value); }
        }
        public string PriceCurrency
        {
            get { return GetValue(() => PriceCurrency); }
            set { SetValue(() => PriceCurrency, value); }
        }
        public string ProductDate
        {
            get { return GetValue(() => ProductDate); }
            set { SetValue(() => ProductDate, value); }
        }
        public string ShippingPrice
        {
            get { return GetValue(() => ShippingPrice); }
            set { SetValue(() => ShippingPrice, value); }
        }

        public string ShippingCurrency
        {
            get { return GetValue(() => ShippingCurrency); }
            set { SetValue(() => ShippingCurrency, value); }
        }
        public string StoreName
        {
            get { return GetValue(() => StoreName); }
            set { SetValue(() => StoreName, value); }
        }
        public string Stock
        {
            get { return GetValue(() => Stock); }
            set { SetValue(() => Stock, value); }
        }
    }
}
