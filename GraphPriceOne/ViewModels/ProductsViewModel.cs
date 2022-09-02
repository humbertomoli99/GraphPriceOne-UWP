using CommunityToolkit.Mvvm.ComponentModel;
using GraphPriceOne.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GraphPriceOne.ViewModels
{
    public class ProductsViewModel : ObservableObject
    {
        public ObservableCollection<ProductInfo> Source { get; } = new ObservableCollection<ProductInfo>();

        public ProductsViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await App.PriceTrackerService.GetProductsAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
