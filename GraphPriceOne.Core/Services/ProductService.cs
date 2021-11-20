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
    public class ProductService : IProductRepository
    {
        public SQLiteAsyncConnection _database;
        public ProductService(string dbPath)
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
            else
            {
                _database.CreateTableAsync<ProductInfo>().Wait();
                _database.CreateTableAsync<Store>().Wait();
                _database.CreateTableAsync<Selectores>().Wait();
                _database.CreateTableAsync<ProductPhotos>().Wait();
                _database.CreateTableAsync<Notification>().Wait();
                _database.CreateTableAsync<History>().Wait();
            }
        }
        //Insert & Update
        public async Task<bool> AddProductAsync(ProductInfo productService)
        {
            if (productService.ID_PRODUCT > 0)
            {
                await _database.UpdateAsync(productService);
            }
            else
            {
                await _database.InsertAsync(productService);
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

        public async Task<bool> UpdateProductAsync(ProductInfo productService)
        {
            await _database.UpdateAsync(productService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }
    }
}
