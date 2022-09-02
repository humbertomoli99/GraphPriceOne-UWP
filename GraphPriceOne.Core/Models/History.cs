using SQLite;

namespace GraphPriceOne.Core.Models
{
    public class History
    {
        [PrimaryKey, AutoIncrement]
        public int ID_HISTORY { get; set; }
        public int STORE_ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public double? PriceTag { get; set; }
        public int? PriceDesc { get; set; }
        public string ProductDate { get; set; }
        public double? ShippingPrice { get; set; }
        public int? Stock { get; set; }
    }
}
