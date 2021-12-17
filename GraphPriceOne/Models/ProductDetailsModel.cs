using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Models
{
    public class ProductDetailsModel : PropertyChangedNotification
    {
        private static ProductsModel ProductSelect;
        public ProductsModel SelectedProduct
        {
            get { return ProductSelect; }
            set { ProductSelect = value; }
        }
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
        public string productHistory
        {
            get { return GetValue(() => productHistory); }
            set { SetValue(() => productHistory, value); }
        }

        public string ImagePath
        {
            get { return GetValue(() => ImagePath); }
            set { SetValue(() => ImagePath, value); }
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
        public string PriceTag
        {
            get { return GetValue(() => PriceTag); }
            set { SetValue(() => PriceTag, value); }
        }
        public string shippingPrice
        {
            get { return GetValue(() => shippingPrice); }
            set { SetValue(() => shippingPrice, value); }
        }
        public string stock
        {
            get { return GetValue(() => stock); }
            set { SetValue(() => stock, value); }
        }
    }
}
