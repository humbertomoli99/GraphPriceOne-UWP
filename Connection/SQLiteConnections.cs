using GraphPriceOne.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class SQLiteConnections
    {
        private SQLiteConnection connection;
        public string path;
        public SQLiteConnections()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "GraphPriceOne1.db3");
            if (!File.Exists(path))
            {
                var StoresList = DefaultData.AllDefaultStores().ToList();
                var SelectoresList = DefaultData.AllDefaultSelectores().ToList();

                connection = new SQLiteConnection(path, false);

                connection.CreateTable<Store>();
                connection.CreateTable<Selector>();

                foreach (var item in StoresList)
                {
                    connection.Insert(item);
                }
                foreach (var item in SelectoresList)
                {
                    connection.Insert(item);
                }
            }
            else
            {
                connection = new SQLiteConnection(path, false);
                connection.CreateTable<ProductInfo>();
                connection.CreateTable<Selector>();
                connection.CreateTable<Store>();
                connection.CreateTable<History>();
                connection.CreateTable<Notification>();
                connection.CreateTable<ProductPhotos>();
            }
            //var TStore = connection.Table<STORE>().ToList();
        }
        public SQLiteConnection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
