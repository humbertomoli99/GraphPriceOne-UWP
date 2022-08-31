using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphPriceOne.Models
{
    public class AddSelectorsModel : PropertyChangedNotification
    {
        public List<AddSelectorsModel> ListSelector
        {
            get { return GetValue(() => ListSelector); }
            set { SetValue(() => ListSelector, value); }
        }
        public int ID_SELECTOR
        {
            get { return GetValue(() => ID_SELECTOR); }
            set { SetValue(() => ID_SELECTOR, value); }
        }
        private static StoresModel StoreSelect;
        public StoresModel SelectedStore
        {
            get { return StoreSelect; }
            set { StoreSelect = value; }
        }
        public BitmapImage IconBitmap
        {
            get { return GetValue(() => IconBitmap); }
            set { SetValue(() => IconBitmap, value); }
        }
        public SvgImageSource IconSvg
        {
            get { return GetValue(() => IconSvg); }
            set { SetValue(() => IconSvg, value); }
        }
        public string startUrl
        {
            get { return GetValue(() => startUrl); }
            set { SetValue(() => startUrl, value); }
        }
        public string nameStore
        {
            get { return GetValue(() => nameStore); }
            set { SetValue(() => nameStore, value); }
        }
        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }
        public string TitleGetAttribute
        {
            get { return GetValue(() => TitleGetAttribute); }
            set { SetValue(() => TitleGetAttribute, value); }
        }
        public bool TitleNotNull
        {
            get { return GetValue(() => TitleNotNull); }
            set { SetValue(() => TitleNotNull, value); }
        }
        public string Description
        {
            get { return GetValue(() => Description); }
            set { SetValue(() => Description, value); }
        }
        public string DescriptionGetAttribute
        {
            get { return GetValue(() => DescriptionGetAttribute); }
            set { SetValue(() => DescriptionGetAttribute, value); }
        }
        public bool DescriptionNotNull
        {
            get { return GetValue(() => DescriptionNotNull); }
            set { SetValue(() => DescriptionNotNull, value); }
        }
        public string Images
        {
            get { return GetValue(() => Images); }
            set { SetValue(() => Images, value); }
        }
        public bool ImagesNotNull
        {
            get { return GetValue(() => ImagesNotNull); }
            set { SetValue(() => ImagesNotNull, value); }
        }
        public string Price
        {
            get { return GetValue(() => Price); }
            set { SetValue(() => Price, value); }
        }
        public string PriceGetAttribute
        {
            get { return GetValue(() => PriceGetAttribute); }
            set { SetValue(() => PriceGetAttribute, value); }
        }
        public bool PriceNotNull
        {
            get { return GetValue(() => PriceNotNull); }
            set { SetValue(() => PriceNotNull, value); }
        }
        public string CurrencyPrice
        {
            get { return GetValue(() => CurrencyPrice); }
            set { SetValue(() => CurrencyPrice, value); }
        }
        public string CurrencyPriceGetAttribute
        {
            get { return GetValue(() => CurrencyPriceGetAttribute); }
            set { SetValue(() => CurrencyPriceGetAttribute, value); }
        }
        public bool CurrencyPriceNotNull
        {
            get { return GetValue(() => CurrencyPriceNotNull); }
            set { SetValue(() => CurrencyPriceNotNull, value); }
        }
        public string Shipping
        {
            get { return GetValue(() => Shipping); }
            set { SetValue(() => Shipping, value); }
        }
        public string ShippingGetAttribute
        {
            get { return GetValue(() => ShippingGetAttribute); }
            set { SetValue(() => ShippingGetAttribute, value); }
        }
        public bool ShippingNotNull
        {
            get { return GetValue(() => ShippingNotNull); }
            set { SetValue(() => ShippingNotNull, value); }
        }
        public string ShippingCurrency
        {
            get { return GetValue(() => ShippingCurrency); }
            set { SetValue(() => ShippingCurrency, value); }
        }
        public string ShippingCurrencyGetAttribute
        {
            get { return GetValue(() => ShippingCurrencyGetAttribute); }
            set { SetValue(() => ShippingCurrencyGetAttribute, value); }
        }
        public bool ShippingCurrencyNotNull
        {
            get { return GetValue(() => ShippingCurrencyNotNull); }
            set { SetValue(() => ShippingCurrencyNotNull, value); }
        }
        public string Stock
        {
            get { return GetValue(() => Stock); }
            set { SetValue(() => Stock, value); }
        }
        public string StockGetAttribute
        {
            get { return GetValue(() => StockGetAttribute); }
            set { SetValue(() => StockGetAttribute, value); }
        }
        public bool StockNotNull
        {
            get { return GetValue(() => StockNotNull); }
            set { SetValue(() => StockNotNull, value); }
        }
        public string SitemapTittle
        {
            get { return GetValue(() => SitemapTittle); }
            set { SetValue(() => SitemapTittle, value); }
        }
    }
}
