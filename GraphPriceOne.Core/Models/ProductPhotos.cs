using SQLite;

namespace GraphPriceOne.Core.Models
{
    public class ProductPhotos
    {
        [PrimaryKey, AutoIncrement]
        public int ID_PHOTO { get; set; }
        public string PhotoSrc { get; set; }
        public int ID_PRODUCT { get; set; }
    }
}
