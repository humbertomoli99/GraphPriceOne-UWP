using CommunityToolkit.Mvvm.ComponentModel;
using GraphPriceOne.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GraphPriceOne.ViewModels
{
    public class ExportViewModel : ObservableObject
    {
        public ObservableCollection<Store> Source { get; } = new ObservableCollection<Store>();

        public ExportViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await App.PriceTrackerService.GetStoresAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
