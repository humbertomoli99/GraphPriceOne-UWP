using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GraphPriceOne.Core.Models
{
    public class ProductInfo
    {
        [PrimaryKey, AutoIncrement]
        public int ID_PRODUCT { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productUrl { get; set; }
        public double? PriceTag { get; set; }
        public string PriceCurrency { get; set; }
        public string ProductDate { get; set; }
        public double? ShippingPrice { get; set; }
        public string ShippingCurrency { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public int? Stock { get; set; }
        public string Image { get; set; }
        [ForeignKey(typeof(Store))]
        public int ID_STORE { get; set; }
        [OneToOne]
        public Store STORE { get; set; }
    }
}
