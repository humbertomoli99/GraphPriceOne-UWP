using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphPriceOne.Core.Models
{
    public class Store
    {
        [PrimaryKey]
        public int ID_STORE { get; set; }
        public string nameStore { get; set; }
        public string startUrl { get; set; }
        public string image { get; set; }
    }
}
