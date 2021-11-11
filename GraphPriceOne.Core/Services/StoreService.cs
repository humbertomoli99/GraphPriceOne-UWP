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
            await _database.DeleteAsync<ProductInfo>(id);
            return await Task.FromResult(true);
        }

        public async Task<ProductInfo> GetProductAsync(int id)
        {
            return await _database.Table<ProductInfo>().Where(p => p.ID_PRODUCT == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductInfo>> GetProductsAsync()
        {
            return await Task.FromResult(await _database.Table<ProductInfo>().ToListAsync());
        }

        public async Task<bool> UpdateProductAsync(ProductInfo storeService)
        {
            await _database.UpdateAsync(storeService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }
    }
}
