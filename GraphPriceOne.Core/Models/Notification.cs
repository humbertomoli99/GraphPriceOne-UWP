using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphPriceOne.Core.Models
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int ID_PRODUCT { get; set; }
        public int Title { get; set; }
        public int Message { get; set; }
    }
}
