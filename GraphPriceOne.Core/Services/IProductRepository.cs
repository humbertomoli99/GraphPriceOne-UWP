using GraphPriceOne.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public interface IProductRepository
    {
        Task<bool> AddProductAsync(Product productService);
        Task<bool> UpdateProductAsync(Product productService);
        Task<bool> DeleteProductAsync(int id);
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
