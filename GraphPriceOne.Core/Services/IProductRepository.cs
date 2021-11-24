using GraphPriceOne.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public interface IProductRepository
    {
        Task<bool> AddProductAsync(ProductInfo productService);
        Task<bool> UpdateProductAsync(ProductInfo productService);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductInfo> GetProductAsync(int id);
        Task<IEnumerable<ProductInfo>> GetProductsAsync();
        
        Task<bool> AddStoreAsync(Store productService);
        Task<bool> UpdateStoreAsync(Store productService);
        Task<bool> DeleteStoreAsync(int id);
        Task<Store> GetStoreAsync(int id);
        Task<IEnumerable<Store>> GetStoresAsync();

        Task<bool> AddSelectorAsync(Selector productService);
        Task<bool> UpdateSelectorAsync(Selector productService);
        Task<bool> DeleteSelectorAsync(int id);
        Task<Selector> GetSelectorAsync(int id);
        Task<IEnumerable<Selector>> GetSelectorsAsync();

        Task<bool> AddHistoryAsync(History productService);
        Task<bool> UpdateHistoryAsync(History productService);
        Task<bool> DeleteHistoryAsync(int id);
        Task<History> GetHistoryAsync(int id);
        Task<IEnumerable<History>> GetHistoriesAsync();
    }
}
