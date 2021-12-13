using GraphPriceOne.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public class PriceTrackerService : IPriceTrackerRepository
    {
        public SQLiteAsyncConnection _database;
        public PriceTrackerService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            if (!File.Exists(dbPath))
            {
                var StoresList = DefaultData.AllDefaultStores().ToList();
                var SelectoresList = DefaultData.AllDefaultSelectores().ToList();

                _database.CreateTableAsync<Store>().Wait();
                _database.CreateTableAsync<Selector>().Wait();

                foreach (var item in StoresList)
                {
                    _database.InsertAsync(item);
                }
                foreach (var item in SelectoresList)
                {
                    _database.InsertAsync(item);
                }
            }
            _database.CreateTableAsync<ProductInfo>().Wait();
            _database.CreateTableAsync<Store>().Wait();
            _database.CreateTableAsync<Selectores>().Wait();
            _database.CreateTableAsync<ProductPhotos>().Wait();
            _database.CreateTableAsync<Notifications>().Wait();
            _database.CreateTableAsync<History>().Wait();
        }

        public async Task<bool> AddHistoryAsync(History PriceTrackerService)
        {
            if (PriceTrackerService.ID_HISTORY > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> AddImageAsync(ProductPhotos PriceTrackerService)
        {
            if (PriceTrackerService.ID_PHOTO > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> AddNotificationAsync(Notifications PriceTrackerService)
        {
            if (PriceTrackerService.PRODUCT_ID > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        //Insert & Update
        public async Task<bool> AddProductAsync(ProductInfo PriceTrackerService)
        {
            if (PriceTrackerService.ID_PRODUCT > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> AddSelectorAsync(Selector PriceTrackerService)
        {
            if (PriceTrackerService.ID_SELECTOR > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> AddStoreAsync(Store PriceTrackerService)
        {
            if (PriceTrackerService.ID_STORE > 0)
            {
                await _database.UpdateAsync(PriceTrackerService);
            }
            else
            {
                await _database.InsertAsync(PriceTrackerService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteHistoryAsync(int id)
        {
            await _database.DeleteAsync<History>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            await _database.DeleteAsync<ProductPhotos>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            await _database.DeleteAsync<Notifications>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            await _database.DeleteAsync<ProductInfo>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSelectorAsync(int id)
        {
            await _database.DeleteAsync<Selector>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            await _database.DeleteAsync<Store>(id);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<History>> GetHistoriesAsync()
        {
            return await Task.FromResult(await _database.Table<History>().ToListAsync());
        }

        public async Task<History> GetHistoryAsync(int id)
        {
            return await _database.Table<History>().Where(p => p.ID_HISTORY == id).FirstOrDefaultAsync();
        }

        public async Task<ProductPhotos> GetImageAsync(int id)
        {
            return await _database.Table<ProductPhotos>().Where(p => p.ID_PHOTO == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductPhotos>> GetImagesAsync()
        {
            return await Task.FromResult(await _database.Table<ProductPhotos>().ToListAsync());
        }

        public async Task<Notifications> GetNotificationAsync(int id)
        {
            return await _database.Table<Notifications>().Where(p => p.PRODUCT_ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notifications>> GetNotificationsAsync()
        {
            return await Task.FromResult(await _database.Table<Notifications>().ToListAsync());
        }

        public async Task<ProductInfo> GetProductAsync(int id)
        {
            return await _database.Table<ProductInfo>().Where(p => p.ID_PRODUCT == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsAsync()
        {
            return await Task.FromResult(await _database.Table<ProductInfo>().ToListAsync());
        }

        public async Task<Selector> GetSelectorAsync(int id)
        {
            return await _database.Table<Selector>().Where(p => p.ID_SELECTOR == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Selector>> GetSelectorsAsync()
        {
            return await Task.FromResult(await _database.Table<Selector>().ToListAsync());
        }

        public async Task<Store> GetStoreAsync(int id)
        {
            return await _database.Table<Store>().Where(p => p.ID_STORE == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresAsync()
        {
            return await Task.FromResult(await _database.Table<Store>().ToListAsync());
        }

        public async Task<bool> UpdateHistoryAsync(History PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateImageAsync(ProductPhotos PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateNotificationAsync(Notifications PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateProductAsync(ProductInfo PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }

        public async Task<bool> UpdateSelectorAsync(Selector PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateStoreAsync(Store PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
        }
    }
}
