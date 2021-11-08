using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphPriceOne.Core.Models
{
    public class History
    {
        [PrimaryKey, AutoIncrement]
        public int ID_HISTORY { get; set; }
        public int STORE_ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public double? priceTag { get; set; }
        public int? priceDesc { get; set; }
        public string productDate { get; set; }
        public double? shippingPrice { get; set; }
        public int? stock { get; set; }
    }
}
