using System.Threading.Tasks;

namespace GraphPriceOne.Library
{
    public class ShippingPrice
    {
        public static async Task GetMercadoLibreShippingPriceAsync(string ProductUrl)
        {
            string url = $"https://www.mercadolibre.com.mx/navigation/addresses-hub?go=https%3A%2F%2Fwww.mercadolibre.com.mx%2Flaptop-huawei-matebook-d15-gris-156-intel-core-i3-10110u-8gb-de-ram-256gb-ssd-intel-uhd-graphics-620-1920x1080px-windows-10-home%2Fp%2FMLM18512986&mode=embed&flow=true&modal=true&zipcode=66610";
        }
    }
}
