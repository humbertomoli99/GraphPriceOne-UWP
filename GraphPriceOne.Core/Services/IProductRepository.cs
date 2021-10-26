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
    }
}
