using GraphPriceOne.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphPriceOne.Core.Services
{
    public class NotificationService
    {
        public SQLiteAsyncConnection _database;

        public async Task<bool> AddNotificationAsync(Notification notificationService)
        {
            if (notificationService.ID_PRODUCT > 0)
            {
                await _database.UpdateAsync(notificationService);
            }
            else
            {
                await _database.InsertAsync(notificationService);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            await _database.DeleteAsync<Notification>(id);
            return await Task.FromResult(true);
        }

        public async Task<Notification> GetNotificationAsync(int id)
        {
            return await _database.Table<Notification>().Where(p => p.ID_PRODUCT == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync()
        {
            return await Task.FromResult(await _database.Table<Notification>().ToListAsync());
        }

        public async Task<bool> UpdateNotificationsAsync(Notification notificationService)
        {
            await _database.UpdateAsync(notificationService);
            return await Task.FromResult(true);
            //throw new NotImplementedException();
        }
    }
}
