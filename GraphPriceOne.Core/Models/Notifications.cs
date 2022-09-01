using SQLite;

namespace GraphPriceOne.Core.Models
{
    public class Notifications
    {
        [PrimaryKey, AutoIncrement]
        public int ID_Notification { get; set; }
        public int PRODUCT_ID { get; set; }
        public int Title { get; set; }
        public string Message { get; set; }
        public double NewPrice { get; set; }
        public double PreviousPrice { get; set; }
    }
}
