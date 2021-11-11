using CsvHelper;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Windows.Storage;

namespace GraphPriceOne.Library
{
    public class ExportData
    {
        public static async void ExportDataCsv()
        {
            var data = await App.ProductService.GetProductsAsync();

            var folder = ApplicationData.Current.LocalFolder.Path;
            var fileName = "ProductList.csv";

            var path = Path.Combine(folder, fileName);

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }

            StorageFolder folderExit = await StorageFolder.GetFolderFromPathAsync(folder);
            StorageFile SaveFile = await folderExit.GetFileAsync(fileName);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker()
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop,
                SuggestedFileName = "Products List"
            };

            savePicker.FileTypeChoices.Add("Spreadsheet", new List<string>() { ".csv" });

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                await SaveFile.CopyAndReplaceAsync(file);
                // write to file

                //await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
    }

}
