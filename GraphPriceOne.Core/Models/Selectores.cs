using SQLite;

namespace GraphPriceOne.Core.Models
{
    public class Selectores
    {
        [PrimaryKey]
        public int ID_SELECTOR { get; set; }
        public string Title { get; set; }
        public string TitleGetAttribute { get; set; }
        public int TitleNotNull { get; set; }
        public string Description { get; set; }
        public string DescriptionGetAttribute { get; set; }
        public int DescriptionNotNull { get; set; }
        public string Images { get; set; }
        public int ImagesNotNull { get; set; }
        public string Price { get; set; }
        public string PriceGetAttribute { get; set; }
        public int PriceNotNull { get; set; }
        public string CurrencyPrice { get; set; }
        public string CurrencyPriceGetAttribute { get; set; }
        public int PriceCurrencyNotNull { get; set; }
        public string Shipping { get; set; }
        public string ShippingGetAttribute { get; set; }
        public int ShippingNotNull { get; set; }
        public string ShippingCurrency { get; set; }
        public string ShippingCurrencyGetAttribute { get; set; }
        public int ShippingCurrencyNotNull { get; set; }
        public string Stock { get; set; }
        public string StockGetAttribute { get; set; }
        public int StockNotNull { get; set; }
    }
}
