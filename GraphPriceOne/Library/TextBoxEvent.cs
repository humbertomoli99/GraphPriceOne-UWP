using System;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml.Input;

namespace GraphPriceOne.Library
{
    public static class TextBoxEvent
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        public static string StripHtml(string source)
        {
            string output, output1, output2, output3, output4;
            //get rid of html tags

            output = Regex.Replace(source, "<br>", "\n");
            output1 = Regex.Replace(output, "<p>", "\n");
            output2 = Regex.Replace(output1, "</p>", "\n");
            output3 = Regex.Replace(output2, "<[^>]*>", string.Empty);
            //get rid of multiple blank lines
            output4 = Regex.Replace(output3, @"^\s*$", string.Empty, RegexOptions.Multiline);
            return output4.Trim();
        }
        public static bool IsValidURL(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "GET";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400)
                    {
                        return true;
                    }
                    else if (statusCode >= 500 && statusCode <= 510)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("The remote server has thrown an internal error.url is not valid: {0}", url));
                        return false;
                    }
                }
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Unhandled status [{0}] returned for url: {1}", ex.Status, url), ex);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Could not test url {0}.", url), ex);
            }
            return false;
        }
        public static void textPreviewKeyDown(KeyRoutedEventArgs e)
        {
            var code = e.KeyStatus;
            // Concidicion que nos permite ingresar solo datos alfanumericos
            if (71 <= code.ScanCode && 83 > code.ScanCode || 2 <= code.ScanCode && 11 >= code.ScanCode)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        public static void numberPreviewKeyDown(KeyRoutedEventArgs e)
        {
            var code = e.KeyStatus;
            // Concidicion que nos permite ingresar solo datos alfanumericos
            if (71 <= code.ScanCode && 83 > code.ScanCode || 2 <= code.ScanCode && 11 >= code.ScanCode || 14 == code.ScanCode)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
