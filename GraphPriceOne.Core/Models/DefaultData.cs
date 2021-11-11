using System;
using System.Collections.Generic;
using System.Text;

namespace GraphPriceOne.Core.Models
{
    public class DefaultData
    {
        public static IEnumerable<Store> AllDefaultStores()
        {
            return new List<Store>()
            {
                new Store()
                {
                    ID_STORE = 1,
                    nameStore = "MercadoLibre Arts",
                    startUrl = "https://articulo.mercadolibre.com.mx/"
                },
                new Store()
                {
                    ID_STORE = 2,
                    nameStore = "MercadoLibre",
                    startUrl = "https://www.mercadolibre.com.mx/"
                },
                new Store()
                {
                    ID_STORE = 3,
                    nameStore = "CyberPuerta",
                    startUrl = "https://www.cyberpuerta.mx/"
                },
                new Store()
                {
                    ID_STORE = 4,
                    nameStore = "Amazon",
                    startUrl = "https://www.amazon.com/"
                },
                new Store()
                {
                    ID_STORE = 5,
                    nameStore = "Amazon MX",
                    startUrl = "https://www.amazon.com.mx/"
                }
            };
        }
        public static IEnumerable<Selector> AllDefaultSelectores()
        {
            return new List<Selector>()
                {
                new Selector()
                {
                    ID_SELECTOR = 1,
                    Title = "#root-app h1.ui-pdp-title",
                    TitleNotNull = 1,
                    Description = "#root-app p.ui-pdp-description__content",
                    Images = "#root-app > div > div.ui-pdp-container.ui-pdp-container--pdp > div > div.ui-pdp-container__col.col-2.ui-pdp-container--column-left.pb-40 > div.ui-pdp-container__row > div > div > div > div.ui-pdp-gallery__column",
                    Price = "meta[itemprop=price]",
                    PriceGetAttribute = "content",
                    PriceNotNull = 1,
                    CurrencyPrice = "#root-app > div > div.ui-pdp-container.ui-pdp-container--pdp > div > div.ui-pdp-container__col.col-1.ui-pdp-container--column-right.mt-16.pr-16.ui-pdp--relative > div > div.ui-pdp-container__row.ui-pdp-component-list.pr-16.pl-16 > div > div.ui-pdp-container__row.ui-pdp-container__row--price > div > div.ui-pdp-price__second-line > span > span.price-tag-text-sr-only",
                    Shipping = "#root-app > div > div.ui-pdp-container.ui-pdp-container--pdp > div > div.ui-pdp-container__col.col-1.ui-pdp-container--column-right.mt-16.pr-16.ui-pdp--relative > div > div.ui-pdp-container__row.ui-pdp-component-list.pr-16.pl-16 > div > div.ui-pdp-container__row.ui-pdp-container__row--shipping-summary > div > div > p > span.price-tag.ui-pdp-price__part > span.price-tag-amount > span.price-tag-fraction",
                    ShippingCurrency = "#root-app > div > div.ui-pdp-container.ui-pdp-container--pdp > div > div.ui-pdp-container__col.col-1.ui-pdp-container--column-right.mt-16.pr-16.ui-pdp--relative > div > div.ui-pdp-container__row.ui-pdp-component-list.pr-16.pl-16 > div > div.ui-pdp-container__row.ui-pdp-container__row--shipping-summary > div > div > p.ui-pdp-color--BLACK.ui-pdp-family--REGULAR.ui-pdp-media__title > span.price-tag.ui-pdp-price__part > span.price-tag-text-sr-only",
                    Stock = "div.andes-tooltip__trigger > div > button > span > span.ui-pdp-buybox__quantity__available",
                },
                new Selector()
                {
                    ID_SELECTOR = 2,
                    Title = "#root-app h1.ui-pdp-title",
                    TitleNotNull = 1,
                    Description = "#root-app > div.ui-pdp > div.ui-pdp-container.ui-pdp-container--pdp > div.ui-pdp-container__row.ui-pdp--relative.ui-pdp-with--separator--fluid.pb-40 > div.ui-pdp-container__col.col-3.ui-pdp-container--column-center.pb-40 > div > div:nth-child(5) > div > div > p",
                    Images = "#root-app > div.ui-pdp > div.ui-pdp-container.ui-pdp-container--pdp > div.ui-pdp-container__row.ui-pdp--relative.ui-pdp-with--separator--fluid.pb-40 > div.ui-pdp-container__col.col-3.ui-pdp-container--column-center.pb-40 > div > div.ui-pdp-container__row.ui-pdp-with--separator--fluid.ui-pdp-with--separator--40 > div.ui-pdp-container__col.col-2.ui-pdp--relative > div > div > div.ui-pdp-gallery__column > span:nth-child(3) > figure",
                    Price = "meta[itemprop=price]",
                    PriceGetAttribute = "content",
                    PriceNotNull = 1,
                    CurrencyPrice = "",
                    Shipping = "#buybox-form > div:nth-child(1) > div > div > div.andes-tooltip__trigger > p",
                    ShippingCurrency = "",
                    Stock = "div.andes-tooltip__trigger > div > button > span > span.ui-pdp-buybox__quantity__available"
                },
                new Selector()
                {
                    ID_SELECTOR = 3,
                    Title = "#productinfo > form > div.detailsInfo.clear > div:nth-child(1) > div:nth-child(2) > div > h1",
                    TitleNotNull = 1,
                    Description = "#productinfo > form > div.detailsInfo.clear > div:nth-child(2) > div:nth-child(2) > div",
                    Images = "#productinfo > form > div.detailsInfo.clear > div:nth-child(1) > div:nth-child(1) > div.detailsInfo_left_picture > div.detailsInfo_left_picture_picture.emzoompics",
                    Price = "#productinfo > form > div.detailsInfo.clear > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(4) > div.medium-7.cell.cp-pr > div > div > div.mainPrice > span",
                    PriceNotNull = 1,
                    CurrencyPrice = "",
                    Shipping = "#productinfo > form > div.detailsInfo.clear > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(4) > div.medium-7.cell.cp-pr > div > div > div.deliverycost > span.deliveryvalue",
                    ShippingCurrency = "",
                    Stock = "#productinfo > form > div.detailsInfo.clear > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(4) > div.medium-7.cell.cp-pr > div > div > div.stock > span > span"
                },
                new Selector()
                {
                    ID_SELECTOR = 4,
                    Title = "#productTitle",
                    TitleNotNull = 1,
                    Description = "#productDescription",
                    Images = "#landingImage",
                    Price = "#priceblock_ourprice",
                    PriceNotNull = 1,
                    CurrencyPrice = "",
                    Shipping = "#priceblock_ourprice_ifdmsg",
                    ShippingCurrency = "",
                    Stock = "#quantity > option:last-child"
                },
                new Selector()
                {
                    ID_SELECTOR = 5,
                    Title = "#productTitle",
                    TitleNotNull = 1,
                    Description = "#feature-bullets",
                    Images = "#landingImage",
                    Price = "#priceblock_ourprice",
                    PriceNotNull = 1,
                    CurrencyPrice = "",
                    Shipping = "span > div > div:nth-child(3) > span",
                    ShippingCurrency = "",
                    Stock = "#quantity > option:last-child"
                }
            };
        }
    }
}
