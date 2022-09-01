using GraphPriceOne.Core.Models;
using System.Collections.Generic;
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

        Task<bool> AddNotificationAsync(Notifications PriceTrackerService);
        Task<bool> UpdateNotificationAsync(Notifications PriceTrackerService);
        Task<bool> DeleteNotificationAsync(int id);
        Task<Notifications> GetNotificationAsync(int id);
        Task<IEnumerable<Notifications>> GetNotificationsAsync();

        Task<bool> AddImageAsync(ProductPhotos PriceTrackerService);
        Task<bool> UpdateImageAsync(ProductPhotos PriceTrackerService);
        Task<bool> DeleteImageAsync(int id);
        Task<ProductPhotos> GetImageAsync(int id);
        Task<IEnumerable<ProductPhotos>> GetImagesAsync();
    }
}
