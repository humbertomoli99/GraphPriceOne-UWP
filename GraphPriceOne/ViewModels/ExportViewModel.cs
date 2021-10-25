using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using GraphPriceOne.Core.Models;
using GraphPriceOne.Core.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GraphPriceOne.ViewModels
{
    public class ExportViewModel : ObservableObject
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ExportViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await SampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
