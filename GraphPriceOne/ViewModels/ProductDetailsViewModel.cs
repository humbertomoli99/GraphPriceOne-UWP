using GraphPriceOne.Library;
using GraphPriceOne.Models;
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

            ListImages = new ObservableCollection<string>() { };
            foreach (var item in ProductImages)
            {
                ListImages.Add(LocalState + item.PhotoSrc);
            }
            
            if(Product.shippingPrice == null)
            {
                shippingPrice = "Not Available";
            }
            else
            {
                shippingPrice = (Product.shippingPrice <= 0)? "Free Shipping" : shippingCurrency + ProductHistory[lastItem].shippingPrice;
            }
            
            stock = (Product.stock == null)? "Stock: Not Available" : "Stock: " + ProductHistory[lastItem].stock;
        }
    }
}
