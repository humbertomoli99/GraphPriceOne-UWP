using GraphPriceOne.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
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
            _database.CreateTableAsync<Product>().Wait();
        }
        //Insert & Update
        public async Task<bool> AddProductAsync(Product productService)
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
            await _database.DeleteAsync<Product>(id);
            return await Task.FromResult(true);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _database.Table<Product>().Where(p => p.ID_PRODUCT == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(await _database.Table<Product>().ToListAsync());
        }

        public Task<bool> UpdateProductAsync(Product productService)
        {
            throw new NotImplementedException();
        }
    }
}
