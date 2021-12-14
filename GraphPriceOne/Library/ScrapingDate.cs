using Fizzler.Systems.HtmlAgilityPack;
using GraphPriceOne.Core.Models;
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
using Windows.UI.Notifications;

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
        public async static Task<string> GetHistory()
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
                            productDate = DateTime.UtcNow.ToString(),
                            stock = ScrapingDate.GetStock(HtmlUrl, SitemapSelectors.Stock, SitemapSelectors.StockGetAttribute),
                            priceTag = ScrapingDate.GetPrice(HtmlUrl, SitemapSelectors.Price, SitemapSelectors.PriceGetAttribute),
                            shippingPrice = ScrapingDate.GetShippingPrice(HtmlUrl, SitemapSelectors.Shipping, SitemapSelectors.ShippingGetAttribute)
                        };

                        await App.PriceTrackerService.AddHistoryAsync(ProductHistory);

                        System.Diagnostics.Debug.WriteLine(i + " to " + Products.ToList().Count);
                        i++;
                    }
                }
                return "Task completed";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static async Task NotifyPriceChangeAsync()
        {
            try
            {
                var Products = await App.PriceTrackerService.GetProductsAsync();
                var i = 1;
                Notifications Notify = new Notifications();
                foreach (var item in Products)
                {
                    //detectar precio anterior
                    var Histories = await App.PriceTrackerService.GetHistoriesAsync();
                    var HistorySelected = Histories.ToList().Where(s => s.PRODUCT_ID.Equals(item.ID_PRODUCT)).ToList();

                    if (HistorySelected.Count > 2)
                    {
                        var lastItem = HistorySelected.Count - 1;

                        var newPrice = HistorySelected[lastItem].priceTag;
                        var previousPrice = HistorySelected[lastItem - 1].priceTag;

                        var ProductSelected = Products.Where(s => s.ID_PRODUCT.Equals(item.ID_PRODUCT)).ToList().First();

                        if (newPrice < previousPrice)
                        {
                            ShowToastNotification("📉 Dropped \n" + ProductSelected.productName, "\n (" + previousPrice + " to " + newPrice + ")");
                            Notify.Message = "📉 Dropped \n" + ProductSelected.productName + "\n (" + previousPrice + " to " + newPrice + ")";
                            Notify.PRODUCT_ID = ProductSelected.ID_PRODUCT;
                        }
                        else if (newPrice > previousPrice)
                        {
                            ShowToastNotification("📈 Increased \n" + ProductSelected.productName, "\n (" + previousPrice + " to " + newPrice + ")");
                            Notify.Message = "📈 Increased \n" + ProductSelected.productName + "\n (" + previousPrice + " to " + newPrice + ")";
                            Notify.PRODUCT_ID = ProductSelected.ID_PRODUCT;
                        }
                        await App.PriceTrackerService.AddNotificationAsync(Notify);
                        System.Diagnostics.Debug.WriteLine(i + " to " + Products.ToList().Count);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private static void ShowToastNotification(string title, string stringContent)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            //toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
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
            try
            {
                WebClient oClient = new WebClient();
                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                string productsIcons = Directory.CreateDirectory(LocalState + Folder).ToString();

                List<string> ListUrl = new List<string>();
                if (Imagen != null)
                {
                    var i = 0;
                    foreach (var lista2 in Imagen)
                    {
                        string format = Path.GetExtension(lista2);
                        //formato de salida
                        string path = Path.Combine(productsIcons, NameFile + i + format);
                        if (lista2 != null && lista2 != string.Empty && !lista2.StartsWith("data:"))
                        {
                            var lista3 = lista2;
                            if (lista2.StartsWith("//"))
                            {
                                lista3 = "https:" + lista2;
                            }

                            if (!TextBoxEvent.IsValidURL(lista2))
                            {
                                Uri uri = new Uri(lista2);
                                lista3 = "https://" + uri.Host + lista2.ToString();
                            }

                            if (lista2.Contains("?"))
                            {
                                lista3 = lista2.Split(new Char[] { '?' })[0];
                                path = path.Split(new Char[] { '?' })[0];
                            }
                            oClient.DownloadFile(new Uri(lista3), path);
                            ListUrl.Add(Path.Combine(Folder, NameFile + i + format));

                            string[] str = ListUrl.ToArray();
                            i++;
                            return str;
                        }
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string DownloadMetaIcon(string URL, ScrapingDate.EnlaceImage urlImage, string folder, string name)
        {
            try
            {
                WebClient oClient = new WebClient();

                string LocalState = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

                string storesIcons = Directory.CreateDirectory(LocalState + folder).ToString();

                //cambiar este valor
                string format = Path.GetExtension(urlImage.href);

                //formato de salida
                string path = Path.Combine(storesIcons, name + urlImage.type);

                if (urlImage != null)
                {
                    if (urlImage.href.StartsWith("//"))
                    {
                        urlImage.href = "https:" + urlImage;
                    }

                    if (!TextBoxEvent.IsValidURL(urlImage.href))
                    {
                        Uri uri = new Uri(URL);
                        urlImage.href = "https://" + uri.Host + urlImage.href.ToString();
                    }

                    if (urlImage.href.Contains("?"))
                    {
                        urlImage.href = urlImage.href.Split(new Char[] { '?' })[0];
                        path = path.Split(new Char[] { '?' })[0];
                    }
                    oClient.DownloadFile(new Uri(urlImage.href), path);
                    return name + urlImage.type;
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
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
        public static string GetTitle(HtmlNode DocumentNode, string XPath)
        {
            var producto = new ProductInfo();
            producto.productName = DocumentNode?.QuerySelector("head > title")?.InnerHtml?.ToString();
            if (XPath != null)
            {
                String withDoubleQuotes = XPath?.Replace("\"", "'");
                producto.productName = DocumentNode?.QuerySelector(withDoubleQuotes)?.InnerText?.Trim();
                return producto.productName;
            }
            return producto.productName;
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
                        producto.stock = 1;
                    }
                    else
                    {
                        string stockArray = new String(selector.Where(Char.IsDigit).ToArray());
                        producto.stock = int.Parse(stockArray);
                    }
                }
                else
                {
                    producto.stock = null;
                }
            }
            else
            {
                string selector = DocumentNode?.QuerySelector(withDoubleQuotes)?.GetAttributeValue(GetAttribute, "");
                producto.stock = int.Parse(selector);
            }
            return producto.stock;
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
                        producto.shippingPrice = 0;
                    }
                    else if (Price.Contains("Envío a todo el país"))
                    {
                        producto.shippingPrice = null;
                    }
                    else
                    {
                        string Price0 = Regex.Replace(Price, "[A-z]", "");
                        string Price1 = Regex.Replace(Price0, "[A-z]", "");
                        string Price2 = Regex.Replace(Price1, @"[$,]", "");
                        string Price3 = Regex.Replace(Price2, "í", "").Trim();
                        producto.shippingPrice = double.Parse(Price3);// return ""
                        return producto.shippingPrice;
                    }
                }
                else
                {
                    producto.shippingPrice = double.Parse(DocumentNode?.QuerySelector(XPath).GetAttributeValue(GetAttribute, ""));
                }
            }
            else
            {
                producto.shippingPrice = null;
            }
            return producto.shippingPrice;
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
            catch (Exception)
            {
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
                producto.productDate = horaCaptura;
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
                    producto.shippingPrice = double.Parse(Regex.Replace(amazonShipping2, @"", ""));
                }

                //MercadoLibre
                var stockMl = document?.DocumentNode?.SelectSingleNode("//span[@class='ui-pdp-buybox__quantity__available']")?.InnerHtml?.ToString();
                if (stockMl != null)
                {
                    //string stockMl1 = Regex.Replace(stockMl, @"\d+", "");
                    string stockMlArray = new String(stockMl.Where(Char.IsDigit).ToArray());
                    producto.stock = int.Parse(stockMlArray);
                }
                var stockMlOne = document?.DocumentNode?.SelectSingleNode("//p[@class='ui-pdp-color--BLACK ui-pdp-size--MEDIUM ui-pdp-family--SEMIBOLD']")?.InnerHtml?.ToString();
                if (stockMlOne != null)
                {
                    int stockMlOne1 = 1;
                    producto.stock = stockMlOne1;
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
                                producto.priceCurrency = PriceCurrency;
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
                                producto.shippingPrice = Int32.Parse(DeliveryChargePrice);
                            }

                            //divisa de precio del envio
                            string DeliveryChargeCurrency = (string)rss?["shipping_promise"]?["price"]?["currency_id"]?.ToString();
                            if (DeliveryChargeCurrency != null)
                            {
                                producto.shippingCurrency = DeliveryChargeCurrency;
                            }
                            else
                            {
                                producto.shippingCurrency = producto.priceCurrency;
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
                            producto.shippingPrice = Int32.Parse(DeliveryChargePrice);
                        }

                        //divisa de precio del envio
                        string DeliveryChargeCurrency = (string)rss2?["offers"]?["priceSpecification"]?["priceCurrency"]?.ToString();
                        if (DeliveryChargeCurrency != null)
                        {
                            producto.shippingCurrency = DeliveryChargeCurrency;
                        }

                        string StockString = (string)rss2?["article"]?["stock"]?.ToString();
                        if (StockString != null)
                        {
                            //convirtiendo string stock a int
                            producto.stock = Int32.Parse(Regex.Replace(StockString, @"", ""));
                        }
                    }
                    return producto;
                }
                return producto;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }
    }
}
