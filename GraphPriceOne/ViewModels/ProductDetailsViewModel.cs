using GraphPriceOne.Library;
using GraphPriceOne.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GraphPriceOne.ViewModels
{
    public class ProductDetailsViewModel : ProductDetailsModel
    {

        public ProductDetailsViewModel()
        {
            if (SelectedProduct != 0)
            {
                _ = GetProductsAsync();
            }
        }

        private async Task GetProductsAsync()
        {
            try
            {
                var Histories = await App.PriceTrackerService.GetHistoriesAsync();
                var ProductHistoryList = Histories.Where(u => u.PRODUCT_ID.Equals(SelectedProduct)).ToList();

                int NumberOfRecords = ProductHistoryList.Count;

                double?[] SumProductPrice = new double?[NumberOfRecords];

                var i = 0;
                foreach (var item in ProductHistoryList)
                {
                    SumProductPrice[i] = double.Parse(item.PriceTag.ToString());
                    productHistory += "Price: " + item.PriceTag + "  Shipping: " + item.ShippingPrice + "  Stock: " + item.Stock + "   Date: " + item.ProductDate + "\n";
                    i++;
                }

                //Product price estadisticas
                double? AvgProductPrice = SumProductPrice.Average();
                double? MinProductPrice = SumProductPrice.Min();
                double? MaxProductPrice = SumProductPrice.Max();

                ShowAvgProductPrice = AvgProductPrice.ToString();
                ShowMinProductPrice = MinProductPrice.ToString();
                ShowMaxProductPrice = MaxProductPrice.ToString();

                ID_PRODUCT = SelectedProduct;
                var Product = await App.PriceTrackerService.GetProductAsync(SelectedProduct);

                productName = Product.productName;
                productUrl = Product.productUrl;
                var lastItem = ProductHistoryList.Count - 1;

                var descript = Product.productDescription;
                productDescription = (descript == null) ? "" : TextBoxEvent.StripHtml(descript);

                var priceCurrency = Product.PriceCurrency;
                priceCurrency = (priceCurrency == null) ? "$" : priceCurrency;
                PriceTag = priceCurrency + ProductHistoryList[lastItem].PriceTag;

                var shippingCurrency = Product.ShippingCurrency;
                shippingCurrency = (shippingCurrency == null) ? "$" : shippingCurrency;

                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                var Images = await App.PriceTrackerService.GetImagesAsync();
                var ProductImages = Images.Where(u => u.ID_PRODUCT.Equals(Product.ID_PRODUCT)).ToList();

                ListImages = new ObservableCollection<string>() { };
                foreach (var item in ProductImages)
                {
                    ListImages.Add(LocalState + item.PhotoSrc);
                }

                if (Product.ShippingPrice == null)
                {
                    shippingPrice = "Not Available";
                }
                else
                {
                    shippingPrice = (Product.ShippingPrice <= 0) ? "Free Shipping" : shippingCurrency + ProductHistoryList[lastItem].ShippingPrice;
                }

                stock = (Product.Stock == null) ? "Stock: Not Available" : "Stock: " + ProductHistoryList[lastItem].Stock;
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
            }
        }
    }
}
