using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphPriceOne.Core.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID_PRODUCT { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string productUrl { get; set; }
        public double? PriceTag { get; set; }
        public string priceCurrency { get; set; }
        public string productDate { get; set; }
        public double? shippingPrice { get; set; }
        public string shippingCurrency { get; set; }
        public string storeName { get; set; }
        public string status { get; set; }
        public int? stock { get; set; }
        public string Image { get; set; }
        [ForeignKey(typeof(Store))]
        public int ID_STORE { get; set; }
        [OneToOne]
        public Store STORE { get; set; }
    }
}
