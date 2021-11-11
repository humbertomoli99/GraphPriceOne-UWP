using GraphPriceOne.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public class StoreService
    {
        public SQLiteAsyncConnection _database;

        public async Task<bool> AddProductAsync(Store storeService)
        {
            if (storeService.ID_STORE > 0)
            {
                await _database.UpdateAsync(storeService);
            }
            else
            {
                await _database.InsertAsync(storeService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            await _database.DeleteAsync<Store>(id);
            return await Task.FromResult(true);
        }

        public async Task<Store> GetProductAsync(int id)
        {
            return await _database.Table<Store>().Where(p => p.ID_STORE == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Store>> GetProductsAsync()
        {
            return await Task.FromResult(await _database.Table<Store>().ToListAsync());
        }

        public async Task<bool> UpdateProductAsync(Store storeService)
        {
            await _database.UpdateAsync(storeService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }
    }
}
