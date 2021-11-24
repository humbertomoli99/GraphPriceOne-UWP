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
            _database.CreateTableAsync<Notification>().Wait();
            _database.CreateTableAsync<History>().Wait();
        }

        public Task<bool> AddHistoryAsync(History PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddNotificationAsync(Notification PriceTrackerService)
        {
            throw new NotImplementedException();
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

        public Task<bool> AddSelectorAsync(Selector PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddStoreAsync(Store PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteHistoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            await _database.DeleteAsync<ProductInfo>(id);
            return await Task.FromResult(true);
        }

        public Task<bool> DeleteSelectorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<History>> GetHistoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<History> GetHistoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductInfo> GetProductAsync(int id)
        {
            return await _database.Table<ProductInfo>().Where(p => p.ID_PRODUCT == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsAsync()
        {
            return await Task.FromResult(await _database.Table<ProductInfo>().ToListAsync());
        }

        public Task<Selector> GetSelectorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Selector>> GetSelectorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Store> GetStoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Store>> GetStoresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateHistoryAsync(History PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNotificationAsync(Notification PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductAsync(ProductInfo PriceTrackerService)
        {
            await _database.UpdateAsync(PriceTrackerService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }

        public Task<bool> UpdateSelectorAsync(Selector PriceTrackerService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStoreAsync(Store PriceTrackerService)
        {
            throw new NotImplementedException();
        }
    }
}
