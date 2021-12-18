using GraphPriceOne.Library;
using GraphPriceOne.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GraphPriceOne.ViewModels
{
    public class ProductDetailsViewModel : ProductDetailsModel
    {

        public ProductDetailsViewModel()
        {
            if (SelectedProduct != null)
            {
                _ = GetProductsAsync();
            }
        }

        private async Task GetProductsAsync()
        {
            var Histories = await App.PriceTrackerService.GetHistoriesAsync();
            var ProductHistory = Histories.Where(u => u.PRODUCT_ID.Equals(SelectedProduct.ID_PRODUCT)).ToList();
            foreach (var item in ProductHistory)
            {
                productHistory += "Price: " + item.priceTag + "  Shipping: " + item.shippingPrice + "  Stock: " + item.stock + "   Date: " + item.productDate + "\n";
            }
            ID_PRODUCT = SelectedProduct.ID_PRODUCT;
            var Product = await App.PriceTrackerService.GetProductAsync(SelectedProduct.ID_PRODUCT);

            productName = Product.productName;
            productUrl = Product.productUrl;
            var lastItem = ProductHistory.Count - 1;

            var descript = Product.productDescription;
            productDescription = (descript == null) ? "" : TextBoxEvent.StripHtml(descript);

            var priceCurrency = Product.priceCurrency;
            priceCurrency = (priceCurrency == null) ? "$" : priceCurrency;
            PriceTag = priceCurrency + ProductHistory[lastItem].priceTag;

            var shippingCurrency = Product.shippingCurrency;
            shippingCurrency = (shippingCurrency == null) ? "$" : shippingCurrency;

            string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

            var Images = await App.PriceTrackerService.GetImagesAsync();
            var ProductImages = Images.Where(u => u.ID_PRODUCT.Equals(Product.ID_PRODUCT)).ToList();

            //ImagePath = new ObservableCollection<string>()
            //{
            //    ProductImages.First().PhotoSrc,
            //    LocalState + ProductImages.First().PhotoSrc
            //};
            ListImages = new ObservableCollection<string>() { };
            foreach (var item in ProductImages)
            {
                //ListImages = new ObservableCollection<string>() { LocalState + ProductImages.First().PhotoSrc };
                ListImages.Add(LocalState + item.PhotoSrc);
            }

            //if (ProductImages != null && ProductImages.Count != 0)
            //{
            //    //srcImage = LocalState + ProductImages.First().PhotoSrc;
            //}

            //ImagePath = srcImage;

            if (Product.shippingPrice == 0)
            {
                shippingPrice = "Free Shipping";
            }
            else if (Product.shippingPrice == null)
            {
                shippingPrice = "Not Available";
            }
            else
            {
                shippingPrice = shippingCurrency + ProductHistory[lastItem].shippingPrice;
            }

            if (Product.stock == null)
            {
                stock = "Stock: Not Available";
            }
            else
            {
                stock = "Stock: " + ProductHistory[lastItem].stock;
            }
        }
    }
}
