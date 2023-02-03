using Fizzler.Systems.HtmlAgilityPack;
using GraphPriceOne.Core.Models;
using GraphPriceOne.Services;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphPriceOne.Library
{
    public class ScrapingDate
    {
        private static string nodePathCp, nodePathMl, scriptsJson, scriptsJsonCpDetails, resultStock;
        public static History ProductHistory { get; private set; }

        public ScrapingDate()
        {
            //private static JObject jsonFile;
        }
        public async static Task GetHistory()
        {
            try
            {
                var Products = await App.PriceTrackerService.GetProductsAsync();

                var i = 1;
                foreach (var item in Products)
                {
                    HtmlNode HtmlUrl = await ScrapingDate.LoadPageAsync(item.productUrl);
                    ScrapingDate.EnlaceImage icon = ScrapingDate.GetMetaIcon(HtmlUrl);

                    var Sitemaps = await App.PriceTrackerService.GetStoresAsync();
                    var ValidSitemaps = Sitemaps.Where(s => item.productUrl.Contains(s.startUrl)).ToList();

                    if (ValidSitemaps.Count != 0)
                    {
                        var id_sitemap = ValidSitemaps.First().ID_STORE;
                        var Selectors = await App.PriceTrackerService.GetSelectorsAsync();
                        var SitemapSelectors = Selectors.Where(s => s.ID_SELECTOR.Equals(id_sitemap)).ToList().First();

                        ProductHistory = new History()
                        {
                            STORE_ID = id_sitemap,
                            PRODUCT_ID = item.ID_PRODUCT,
                            ProductDate = DateTime.UtcNow.ToString(),
                            Stock = ScrapingDate.GetStock(HtmlUrl, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                            PriceTag = ScrapingDate.GetPrice(HtmlUrl, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                            ShippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                        };

                        await App.PriceTrackerService.AddHistoryAsync(ProductHistory);

                        System.Diagnostics.Debug.WriteLine(i + " to " + Products.ToList().Count);
                        i++;
                    }
                }
                await ScrapingDate.NotifyPriceChangeAsync();
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
            }
        }
        public static async Task NotifyPriceChangeAsync()
        {
            try
            {
                var Products = await App.PriceTrackerService.GetProductsAsync();
                int i = 1;
                Notifications Notify = new Notifications();
                foreach (var item in Products)
                {
                    //detectar precio anterior
                    var Histories = await App.PriceTrackerService.GetHistoriesAsync();
                    var HistorySelected = Histories.ToList().Where(s => s.PRODUCT_ID.Equals(item.ID_PRODUCT)).ToList();

                    if (HistorySelected.Count > 2)
                    {
                        int lastItem = HistorySelected.Count - 1;

                        double? newPrice = HistorySelected[lastItem].PriceTag;
                        double? previousPrice = HistorySelected[lastItem - 1].PriceTag;

                        ProductInfo ProductSelected = Products.Where(s => s.ID_PRODUCT.Equals(item.ID_PRODUCT)).ToList().First();

                        if (newPrice != previousPrice)
                        {
                            var drop = (newPrice < previousPrice) ? "📉 Dropped" : "📈 Increased";

                            var titleNotification = drop + "\n" + ProductSelected.productName;
                            var contentNotification = "\n (" + previousPrice + " to " + newPrice + ")";

                            ToastNotificationsService.ShowToastNotification(titleNotification, contentNotification, ProductSelected);

                            Notify.Message = titleNotification + contentNotification;
                            Notify.PRODUCT_ID = ProductSelected.ID_PRODUCT;
                            Notify.NewPrice = (double)newPrice;
                            Notify.PreviousPrice = (double)previousPrice;

                            await App.PriceTrackerService.AddNotificationAsync(Notify);
                        }
                        System.Diagnostics.Debug.WriteLine(i + " to " + Products.ToList().Count);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
            }
        }
        public static async Task<HtmlNode> LoadPageAsync(string RequestUri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(RequestUri);
            HttpContent content = response.Content;
            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(await content.ReadAsStringAsync());
            return document.DocumentNode;
        }

        public static List<string> GetUrls(HtmlNode htmlUrl)
        {
            List<string> ListUrl = new List<string>();

            foreach (var nodo in htmlUrl.QuerySelectorAll("a"))
            {
                ListUrl.Add(nodo.GetAttributeValue("href", ""));
            }
            return ListUrl;
        }

        public static Task<string> QuerySelectorAsync(string selector, HtmlNode DocumentNode)
        {
            return Task.FromResult(DocumentNode.QuerySelector(selector).InnerHtml);
        }
        public class EnlaceImage
        {
            public string href { get; set; }
            public string type { get; set; }
        }
        public static EnlaceImage GetMetaIcon(HtmlNode DocumentNode)
        {
            HtmlNode LinkIcon = DocumentNode?.SelectSingleNode("//link[@rel='icon' or @rel='shortcut icon']");
            HtmlNode MetaIcon = DocumentNode?.SelectSingleNode("//meta[@itemprop='image' or @property='og:image']"); //<meta content="" itemprop="image">

            EnlaceImage srcIcono = new EnlaceImage
            {
                href = LinkIcon?.GetAttributeValue("href", "")?.ToString(),
                type = LinkIcon?.GetAttributeValue("type", "")?.ToString()
            };

            //if (srcIcono.href != null || srcIcono.type != null)
            //{
            if (LinkIcon != null)
            {
                //borrar variables de url
                if (srcIcono.href.Contains("?"))
                {
                    srcIcono.href = srcIcono.href.Split(new Char[] { '?' })[0];
                }
                else if (LinkIcon != null)
                {
                    if (srcIcono.type != string.Empty)
                    {
                        if (srcIcono.type == "image/jpeg") { srcIcono.type = ".jpeg"; }
                        if (srcIcono.type == "image/svg+xml") { srcIcono.type = ".svg"; }
                        if (srcIcono.type == "image/svg") { srcIcono.type = ".svg"; }
                        if (srcIcono.type == "image/png") { srcIcono.type = ".png"; }
                        if (srcIcono.type == "image/icon" || srcIcono.type == "image/x-icon") { srcIcono.type = ".ico"; }
                    }
                    else
                    {
                        srcIcono.type = Path.GetExtension(srcIcono.href);
                    }
                    return srcIcono;
                }
                //}
                if (MetaIcon != null)
                {
                    srcIcono.href = MetaIcon?.GetAttributeValue("content", "");
                    if (srcIcono.href.Contains("?"))
                    {
                        srcIcono.href = srcIcono.href.Split(new Char[] { '?' })[0];
                    }
                    srcIcono.type = Path.GetExtension(srcIcono.href);
                    return srcIcono;
                }
            }
            else
            {
                srcIcono.href = "/favicon.ico";
                srcIcono.type = ".ico";
                return srcIcono;
            }
            return null;
        }
        public static string QueryString(string selector, HtmlDocument doc)
        {
            HtmlNode node = doc.DocumentNode.QuerySelector(selector);
            var url = node.Attributes["href"];
            return node.InnerHtml;
        }
        public static List<string> GetUrlImage(HtmlNode UploadedDocument, string node)
        {
            List<string> ListUrl = new List<string>();
            HtmlNode ElementIcon = UploadedDocument.QuerySelector(node);
            if (ElementIcon != null)
            {
                foreach (var nodo in ElementIcon.QuerySelectorAll("img"))
                {
                    if (!nodo.GetAttributeValue("src", "").StartsWith("data:"))
                    {
                        ListUrl.Add(nodo.GetAttributeValue("src", ""));
                    }
                }
                if (ElementIcon.OuterHtml != null)
                {
                    ListUrl.Add(ElementIcon.GetAttributeValue("src", ""));
                }
                return ListUrl;
            }
            return null;
        }
        public static string[] DownloadImage(string URL, List<string> Imagen, string Folder, string NameFile)
        {
            WebClient oClient = new WebClient();
            string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string productsIcons = Directory.CreateDirectory(LocalState + Folder).ToString();

            List<string> ListUrl = new List<string>();
            if (Imagen == null || Imagen.Count == 0)
            {
                return new string[0];
            }

            for (var i = 0; i < Imagen.Count; i++)
            {
                string format = Path.GetExtension(Imagen[i]);
                //formato de salida
                string path = Path.Combine(productsIcons, NameFile + i + format);
                if (Imagen[i] == null || Imagen[i] == string.Empty || Imagen[i].StartsWith("data:"))
                {
                    continue;
                }

                var lista3 = Imagen[i];
                if (Imagen[i].StartsWith("//"))
                {
                    lista3 = "https:" + Imagen[i];
                }

                if (!TextBoxEvent.IsValidURL(Imagen[i]))
                {
                    Uri uri = new Uri(Imagen[i]);
                    lista3 = "https://" + uri.Host + Imagen[i].ToString();
                }

                if (Imagen[i].Contains("?"))
                {
                    lista3 = Imagen[i].Split(new Char[] { '?' })[0];
                    path = path.Split(new Char[] { '?' })[0];
                }

                try
                {
                    oClient.DownloadFile(new Uri(lista3), path);
                    ListUrl.Add(Path.Combine(Folder, NameFile + i + format));
                }
                catch (Exception)
                {
                    // Do nothing
                }
            }

            return ListUrl.ToArray();
        }

        public async static Task<string> DownloadMetaIcon(string URL, ScrapingDate.EnlaceImage urlImage, string folder, string name)
        {
            try
            {
                // Si no hay enlace a una imagen
                if (urlImage == null)
                {
                    return null;
                }

                WebClient oClient = new WebClient();

                // Obtener la ruta local
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                // Crear la carpeta donde se almacenarán las imágenes
                string storesIcons = Directory.CreateDirectory(LocalState + folder).ToString();

                // Extensión de archivo
                string format = Path.GetExtension(urlImage.href);

                // Formato de salida
                string path = Path.Combine(storesIcons, name + urlImage.type);

                // Si el enlace comienza con "//", agregar "https:"
                if (urlImage.href.StartsWith("//"))
                {
                    urlImage.href = "https:" + urlImage;
                }

                // Si no es una URL válida
                if (!TextBoxEvent.IsValidURL(urlImage.href))
                {
                    Uri uri = new Uri(URL);
                    urlImage.href = "https://" + uri.Host + urlImage.href.ToString();
                }

                // Si hay un signo de interrogación "?" en la URL
                if (urlImage.href.Contains("?"))
                {
                    urlImage.href = urlImage.href.Split(new Char[] { '?' })[0];
                    path = path.Split(new Char[] { '?' })[0];
                }

                // Descargar el archivo
                oClient.DownloadFile(new Uri(urlImage.href), path);
                return name + urlImage.type;
            }
            catch (Exception ex)
            {
                // Mostrar una ventana de excepción
                await Dialogs.ExceptionDialog(ex);
                return null;
            }
        }

        //public static PRODUCT GetPrice(HtmlDocument UploadedDocument, string XPath)
        //{
        //    PRODUCT producto = new PRODUCT();
        //    HtmlNode ElementPrice = UploadedDocument.DocumentNode.SelectSingleNode(XPath);
        //    string StringPrice = ElementPrice.ToString();
        //    producto.PriceTag = StringPrice;
        //    return producto;
        //}
        public static string GetTitle(HtmlNode DocumentNode, string Selector)
        {
            string productName = GetMetaTitle(DocumentNode);

            if (!string.IsNullOrEmpty(Selector))
            {
                string withDoubleQuotes = Selector.Replace("\"", "'");
                HtmlNode productNameNode = DocumentNode.QuerySelector(withDoubleQuotes);
                if (productNameNode != null)
                {
                    productName = productNameNode.InnerText.Trim();
                }
            }

            return productName;
        }

        public static string GetMetaTitle(HtmlNode DocumentNode)
        {
            HtmlNode titleNode = DocumentNode.SelectSingleNode("//head/title");
            return titleNode?.InnerHtml.Trim() ?? string.Empty;
        }

        public static string GetDescription(HtmlNode DocumentNode, string XPath, string GetAttribute = null)
        {
            string GetDescription = "";
            if (XPath != null)
            {
                if (GetAttribute == null || GetAttribute == string.Empty)
                {
                    GetDescription = DocumentNode?.QuerySelector(XPath)?.InnerHtml?.ToString();
                    //GetDescription = DocumentNode?.SelectSingleNode(XPath)?.InnerHtml?.ToString();
                    if (GetDescription == null)
                    {
                        //GetDescription = DocumentNode.QuerySelector("meta[name='description']").GetAttributeValue("content","");
                        GetDescription = DocumentNode?.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", "")?.ToString();
                    }
                }
                else
                {
                    GetDescription = DocumentNode?.QuerySelector(XPath)?.GetAttributeValue(GetAttribute, "")?.ToString();
                }
            }
            else
            {
                GetDescription = DocumentNode?.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", "")?.ToString();
            }
            return GetDescription;
        }
        public static int? GetStock(HtmlNode DocumentNode, string XPath, string GetAttribute = null)
        {
            var producto = new ProductInfo();
            String withDoubleQuotes = XPath.Replace("\"", "'");
            //conversion de string a numeros sin ningun string

            if (GetAttribute == string.Empty || GetAttribute == null)
            {
                string selector = DocumentNode?.QuerySelector(withDoubleQuotes)?.InnerHtml?.ToString();
                if (selector != null)
                {
                    if (selector.Contains("¡Última disponible!") || selector.Contains("Última") || selector.Contains("Ultima"))
                    {
                        producto.Stock = 1;
                    }
                    else
                    {
                        string stockArray = new String(selector.Where(Char.IsDigit).ToArray());
                        producto.Stock = int.Parse(stockArray);
                    }
                }
                else
                {
                    producto.Stock = null;
                }
            }
            else
            {
                string selector = DocumentNode?.QuerySelector(withDoubleQuotes)?.GetAttributeValue(GetAttribute, "");
                producto.Stock = int.Parse(selector);
            }
            return producto.Stock;
        }
        public static double? GetPrice(HtmlNode DocumentNode, string XPath, string GetAttribute = null)
        {
            var producto = new ProductInfo();
            //String withDoubleQuotes1 = XPath.Replace("\"", "'");
            //string Price = DocumentNode?.SelectSingleNode(XPath)?.InnerHtml?.ToString();
            if (GetAttribute == string.Empty || GetAttribute == null)
            {
                string Price = DocumentNode?.QuerySelector(XPath)?.InnerHtml;
                if (Price != null)
                {
                    string Price1 = Regex.Replace(Price, @"[$,]", "");
                    string Price2 = Regex.Replace(Price1, @"", "");
                    producto.PriceTag = double.Parse(Regex.Replace(Price2, @" ", ""));
                }
            }
            else
            {
                string Price = DocumentNode?.QuerySelector(XPath)?.GetAttributeValue(GetAttribute, "");
                if (Price != null)
                {
                    double price1;
                    bool canConvert = double.TryParse(Price, out price1);
                    if (canConvert == true)
                    {
                        string Price1 = Regex.Replace(price1.ToString(), @"[$,]", "");
                        string Price2 = Regex.Replace(Price1, @"", "");
                        string price3 = Regex.Replace(Price2, @" ", "");
                        producto.PriceTag = double.Parse(price3);
                    }
                    else
                    {
                        producto.PriceTag = null;
                    }
                }
            }

            return producto.PriceTag;
        }
        public static double? GetShippingPrice(HtmlNode DocumentNode, string XPath, string GetAttribute = null)
        {
            var producto = new ProductInfo();
            string Price = DocumentNode?.QuerySelector(XPath)?.InnerHtml;

            if (Price != null)
            {
                if (GetAttribute != string.Empty || GetAttribute != null)
                {
                    if (Price.Contains("free") || Price.Contains("gratis") || Price.Contains("GRATUITO"))
                    {
                        producto.ShippingPrice = 0;
                    }
                    else if (Price.Contains("Envío a todo el país"))
                    {
                        producto.ShippingPrice = null;
                    }
                    else
                    {
                        string Price0 = Regex.Replace(Price, "[A-z]", "");
                        string Price1 = Regex.Replace(Price0, "[A-z]", "");
                        string Price2 = Regex.Replace(Price1, @"[$,]", "");
                        string Price3 = Regex.Replace(Price2, "í", "").Trim();
                        producto.ShippingPrice = double.Parse(Price3);// return ""
                        return producto.ShippingPrice;
                    }
                }
                else
                {
                    producto.ShippingPrice = double.Parse(DocumentNode?.QuerySelector(XPath).GetAttributeValue(GetAttribute, ""));
                }
            }
            else
            {
                producto.ShippingPrice = null;
            }
            return producto.ShippingPrice;
        }
        //private static PRODUCT GetPriceCurrency(HtmlDocument UploadedDocument, string XPath)
        //{
        //    var producto = new PRODUCT();
        //    var Price = UploadedDocument?.DocumentNode?.SelectSingleNode(XPath)?.InnerHtml?.ToString();
        //    return 
        //}

        public async static Task<ProductInfo> GetDataByXPathAsync(string RequestUri, string xPath, bool innerText, string attribute)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(RequestUri);
                HttpContent content = response.Content;
                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(await content.ReadAsStringAsync());

                var producto = new ProductInfo();

                //selecciona un elemento con el xpath
                HtmlNode result = document?.DocumentNode?.SelectSingleNode(xPath);

                //obtiene el valor del atributo
                if (attribute != null)
                {
                    resultStock = result?.GetAttributeValue(attribute, "");
                }
                else
                {
                    resultStock = result?.InnerText;
                }
                producto.productName = resultStock;
                return producto;
            }
            catch (Exception ex)
            {
                await Dialogs.ExceptionDialog(ex);
                return null;
            }
        }
        public async static Task<ProductInfo> GetJsonDataAsync(string RequestUri)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(RequestUri);
                HttpContent content = response.Content;
                HtmlDocument document = new HtmlDocument();
                scriptsJson = null;
                scriptsJsonCpDetails = null;

                var producto = new ProductInfo();

                document.LoadHtml(await content.ReadAsStringAsync());

                //datos universales title, fecha y url
                producto.productUrl = RequestUri;


                var Title = document?.DocumentNode?.SelectSingleNode("//title")?.InnerHtml?.ToString();
                producto.productName = Title;

                var horaCaptura = DateTime.UtcNow.ToString();
                producto.ProductDate = horaCaptura;
                //fin datos universales

                //amazon
                var amazonPrice = document?.DocumentNode?.SelectSingleNode("//span[@id='priceblock_ourprice']")?.InnerHtml?.ToString();
                if (amazonPrice != null)
                {
                    string amazonPrice1 = Regex.Replace(amazonPrice, @"[$,]", "");
                    string amazonPrice2 = Regex.Replace(amazonPrice1, @"", "");
                    producto.PriceTag = double.Parse(Regex.Replace(amazonPrice2, @" ", ""));
                }

                var amazonShipping = document?.DocumentNode?.SelectSingleNode("//div[@id='mir-layout-DELIVERY_BLOCK-slot-DELIVERY_MESSAGE']/text()")?.InnerHtml?.ToString();
                if (amazonShipping != null)
                {
                    var amazonShipping1 = Regex.Replace(amazonShipping, @" entrega:", "");
                    var amazonShipping2 = Regex.Replace(amazonShipping1, @"\$", "");
                    producto.ShippingPrice = double.Parse(Regex.Replace(amazonShipping2, @"", ""));
                }

                //MercadoLibre
                var stockMl = document?.DocumentNode?.SelectSingleNode("//span[@class='ui-pdp-buybox__quantity__available']")?.InnerHtml?.ToString();
                if (stockMl != null)
                {
                    //string stockMl1 = Regex.Replace(stockMl, @"\d+", "");
                    string stockMlArray = new String(stockMl.Where(Char.IsDigit).ToArray());
                    producto.Stock = int.Parse(stockMlArray);
                }
                var stockMlOne = document?.DocumentNode?.SelectSingleNode("//p[@class='ui-pdp-color--BLACK ui-pdp-size--MEDIUM ui-pdp-family--SEMIBOLD']")?.InnerHtml?.ToString();
                if (stockMlOne != null)
                {
                    int stockMlOne1 = 1;
                    producto.Stock = stockMlOne1;
                }
                //JObject rssAmaz = JObject.Parse(amazonPrice);
                //string PriceA = (string)rssAmaz?["value"]?.ToString();

                //este objeto json lo usa ML
                var xpath = document?.DocumentNode?.SelectNodes("//script[@type='application/ld+json']");
                if (xpath != null)
                {
                    int root = xpath.Count;

                    //for (int i = 1; i < root + 1; i++)
                    //{
                    //    nodePath = "//script[@type='application/ld+json'][" + i + "]";
                    //    scriptsJson += document.DocumentNode.SelectSingleNode(nodePath).InnerHtml;
                    //}

                    //nodePath2 = "//script[@type=\"application/ld+json\"]";
                    for (int i = 1; i < root + 1; i++)
                    {

                        nodePathMl = "//script[@type='application/ld+json'][" + i + "]";
                        scriptsJson = document?.DocumentNode?.SelectSingleNode(nodePathMl)?.InnerHtml?.ToString();
                        if (scriptsJson != null)
                        {
                            JObject rss = JObject.Parse(scriptsJson);

                            //nombre del producto
                            //string Name = (string)rss?["name"]?.ToString();
                            //if (Name != null)
                            //{
                            //    producto.productName = Name;
                            //}

                            //precio de producto
                            string PriceString = (string)rss?["offers"]?["price"]?.ToString();
                            if (PriceString != null)
                            {
                                producto.PriceTag = double.Parse(Regex.Replace(PriceString, @"", ""));
                            }


                            //divisa precio producto
                            string PriceCurrency = (string)rss?["offers"]?["priceCurrency"]?.ToString();
                            if (PriceCurrency != null)
                            {
                                producto.PriceCurrency = PriceCurrency;
                            }

                            //melidata("add", "event_data"
                            //url del producto desde metaetiqueta
                            //string productUrl = (string)rss?["offers"]?["url"]?.ToString();
                            //if (productUrl != null)
                            //{
                            //    producto.productUrl = productUrl;
                            //}

                            //var priceComplete = PriceCurrency + " " + Price;
                            //var precioString = PriceString.ToString();

                            string DeliveryChargePrice = (string)rss?["shipping_promise"]?["price"]?["amount"]?.ToString();
                            if (DeliveryChargePrice != null)
                            {
                                producto.ShippingPrice = Int32.Parse(DeliveryChargePrice);
                            }

                            //divisa de precio del envio
                            string DeliveryChargeCurrency = (string)rss?["shipping_promise"]?["price"]?["currency_id"]?.ToString();
                            if (DeliveryChargeCurrency != null)
                            {
                                producto.ShippingCurrency = DeliveryChargeCurrency;
                            }
                            else
                            {
                                producto.ShippingCurrency = producto.PriceCurrency;
                            }
                        }
                    }

                    //otro archivo json de CyberPuerta
                    nodePathCp = "//script[@id='cp-details-view-data']";
                    scriptsJsonCpDetails = document?.DocumentNode?.SelectSingleNode(nodePathCp)?.InnerHtml?.ToString();
                    if (scriptsJsonCpDetails != null)
                    {
                        JObject rss2 = JObject.Parse(scriptsJsonCpDetails);
                        //precio de envio
                        string DeliveryChargePrice = (string)rss2?["article"]?["shipping"]?.ToString();
                        if (DeliveryChargePrice != null)
                        {
                            //Int32.Parse(Regex.Replace())
                            producto.ShippingPrice = Int32.Parse(DeliveryChargePrice);
                        }

                        //divisa de precio del envio
                        string DeliveryChargeCurrency = (string)rss2?["offers"]?["priceSpecification"]?["priceCurrency"]?.ToString();
                        if (DeliveryChargeCurrency != null)
                        {
                            producto.ShippingCurrency = DeliveryChargeCurrency;
                        }

                        string StockString = (string)rss2?["article"]?["stock"]?.ToString();
                        if (StockString != null)
                        {
                            //convirtiendo string stock a int
                            producto.Stock = Int32.Parse(Regex.Replace(StockString, @"", ""));
                        }
                    }
                    return producto;
                }
                return producto;
            }
            catch (Exception ex)
            {

                await Dialogs.ExceptionDialog(ex);
                return null;
            }
        }
    }
}
