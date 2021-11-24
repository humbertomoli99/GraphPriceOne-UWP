using GraphPriceOne.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public interface IPriceTrackerRepository
    {
        Task<bool> AddProductAsync(ProductInfo PriceTrackerService);
        Task<bool> UpdateProductAsync(ProductInfo PriceTrackerService);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductInfo> GetProductAsync(int id);
        Task<IEnumerable<ProductInfo>> GetProductsAsync();
        
        Task<bool> AddStoreAsync(Store PriceTrackerService);
        Task<bool> UpdateStoreAsync(Store PriceTrackerService);
        Task<bool> DeleteStoreAsync(int id);
        Task<Store> GetStoreAsync(int id);
        Task<IEnumerable<Store>> GetStoresAsync();

        Task<bool> AddSelectorAsync(Selector PriceTrackerService);
        Task<bool> UpdateSelectorAsync(Selector PriceTrackerService);
        Task<bool> DeleteSelectorAsync(int id);
        Task<Selector> GetSelectorAsync(int id);
        Task<IEnumerable<Selector>> GetSelectorsAsync();

        Task<bool> AddHistoryAsync(History PriceTrackerService);
        Task<bool> UpdateHistoryAsync(History PriceTrackerService);
        Task<bool> DeleteHistoryAsync(int id);
        Task<History> GetHistoryAsync(int id);
        Task<IEnumerable<History>> GetHistoriesAsync();

        Task<bool> AddNotificationAsync(Notification PriceTrackerService);
        Task<bool> UpdateNotificationAsync(Notification PriceTrackerService);
        Task<bool> DeleteNotificationAsync(int id);
        Task<Notification> GetNotificationAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsAsync();
    }
}
