﻿using System.Collections.ObjectModel;

namespace GraphPriceOne.Models
{
    public class ProductDetailsModel : PropertyChangedNotification
    {
        private static int ProductSelect;
        public int SelectedProduct
        {
            get { return ProductSelect; }
            set { ProductSelect = value; }
        }
        public int ID_PRODUCT
        {
            get { return GetValue(() => ID_PRODUCT); }
            set { SetValue(() => ID_PRODUCT, value); }
        }
        public string ShowAvgProductPrice
        {
            get { return GetValue(() => ShowAvgProductPrice); }
            set { SetValue(() => ShowAvgProductPrice, value); }
        }
        public string ShowMinProductPrice
        {
            get { return GetValue(() => ShowMinProductPrice); }
            set { SetValue(() => ShowMinProductPrice, value); }
        }
        public string ShowMaxProductPrice
        {
            get { return GetValue(() => ShowMaxProductPrice); }
            set { SetValue(() => ShowMaxProductPrice, value); }
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
        public ObservableCollection<string> ListImages
        {
            get { return GetValue(() => ListImages); }
            set { SetValue(() => ListImages, value); }
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
