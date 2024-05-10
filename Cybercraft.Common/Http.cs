using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Cybercraft.Common
{
    

    public class Http
    {
        protected const string UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

        public enum Error
        {
            Ok = 0,
            ServiceUnavailable = 503,
            Other,
        }

        public class Response
        {
            public HttpStatusCode Status { get; }

            public byte[] Data { get; }
            public string Text { get; }
            public HttpResponseMessage HttpResponse { get; }

            public Cookie[] SetCookies { get; }
            public Cookie[] AllCookies { get; }

            public bool IsOk => Status == HttpStatusCode.OK && (Data != null || Text != null);

            public Response(HttpStatusCode status, byte[] data, string text, HttpResponseMessage httpResponse, Cookie[] setCookies, Cookie[] allCookies)
            {
                Status = status;
                Data = data;
                Text = text;
                HttpResponse = httpResponse;
                SetCookies = setCookies ?? Array.Empty<Cookie>();
                AllCookies = allCookies ?? Array.Empty<Cookie>();
            }
        }

#region WebClient version
        private class CompressedWebClient : WebClient
        {
            /*private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            public CompressedWebClient() : base()
            {
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            }*/


            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                if (request != null)
                {
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.Headers["Accept-Encoding"] = "gzip";
                }

                return request;
            }
        }

#if NET
        [Obsolete]
#endif
        public static byte[] DownloadData(string url)
        {
            return DownloadData(url, out var _);
        }

#if NET
        [Obsolete]
#endif
        public static byte[] DownloadData(string url, out Error error, int retry = 0)
        {
            using (var client = new CompressedWebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
                    var uri = new Uri(url);
                    client.Headers.Add(HttpRequestHeader.Host, uri.Host);

                    var data = client.DownloadData(url);
                    var dataStr = Encoding.UTF8.GetString(data);
                    error = Error.Ok;
                    return data;
                }
                // ReSharper disable once UnusedVariable
                catch (WebException e)
                {
                    if (retry > 0 || !e.Message.Contains("SSL"))
                        Console.WriteLine("[WEB ERROR] {0}", e.Message);
                    if (e.Message.Contains("SSL"))
                    {
                        if (retry > 0)
                        {
                            Console.WriteLine("            HResult: {0}", e.HResult);
                            Console.WriteLine("            Status:  {0}", e.Status);
                        } else
                        {
                            //Console.WriteLine("Retrying...");
                            Thread.Sleep(250);
                            return DownloadData(url, out error, 1);
                        }
                    } else if (e.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        error = Error.ServiceUnavailable;
                        return null;
                    }

                    error = Error.Other;
                    return null;
                }
            }
        }

#if NET
        [Obsolete]
#endif
        public static string Download(string url)
        {
            return Download(url, out var _);
        }

#if NET
        [Obsolete]
#endif
        public static string Download(string url, out Error error)
        {
            var data = DownloadData(url, out error);
            return data == null ? null : Encoding.UTF8.GetString(data);
        }

#endregion

#region HttpClient version

        public enum DownloadMode
        {
            Unspecified,
            Text,
            Binary,
        }

        public static Response HttpDownload(string url, string referrer = null, Cookie[] cookies = null, DownloadMode downloadMode = DownloadMode.Unspecified, int retry = 0)
        {

            // https://stackoverflow.com/questions/63625732/adding-httpclient-defaultrequestheader-and-requestmessage-header
            // https://stackoverflow.com/questions/28754673/httpclient-conditionally-set-acceptencoding-compression-at-runtime
            // https://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage
            // https://stackoverflow.com/questions/29224734/how-to-read-cookies-from-httpresponsemessage

            bool useCompression = downloadMode == DownloadMode.Text || downloadMode == DownloadMode.Unspecified && (
                !(url.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                  url.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                  url.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                  url.EndsWith(".png", StringComparison.OrdinalIgnoreCase)));

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer, AutomaticDecompression = useCompression ? DecompressionMethods.GZip | DecompressionMethods.Deflate : DecompressionMethods.None })
            using (var client = new HttpClient(handler))
            {
                try
                {
                    // This does no seem to be quite the correct way of writing HttpClient code...

                    var uri = new Uri(url);
                    var baseUri = new Uri(uri.Scheme + "://" + uri.Host);
                    client.BaseAddress = baseUri;

                    client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
                    if (useCompression)
                    {
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                    }

                    client.DefaultRequestHeaders.Host = uri.Host;
                    
                    if (cookies != null)
                    {
                        foreach (var cookie in cookies)
                        {
                            cookieContainer.Add(baseUri, cookie);
                        }
                    }
                    
                    if (referrer != null)
                    {
                        client.DefaultRequestHeaders.Referrer = new Uri(referrer);
                    }


                    var response = client.GetAsync(uri).Result;

                    var returnedCookies = cookieContainer.GetCookies(baseUri).Cast<Cookie>().ToArray();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("[HTTP ERROR] {0} : {1}", (int)response.StatusCode, response.ReasonPhrase);
                        Console.WriteLine("        URL: {0}", url);
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.GatewayTimeout:
                                if (retry == 0)
                                {
                                    Thread.Sleep(30 * 1000);
                                    return HttpDownload(url, referrer, cookies, downloadMode, 1);
                                }
                                break;
                        }

                        return new Response(response.StatusCode, null, null, response, null, returnedCookies);
                    }

                    var data = downloadMode == DownloadMode.Binary || downloadMode == DownloadMode.Unspecified
                        ? response.Content.ReadAsByteArrayAsync().Result
                        : null;

                    var text = downloadMode == DownloadMode.Text || downloadMode == DownloadMode.Unspecified
                        ? response.Content.ReadAsStringAsync().Result
                        : null;

                    var newCookies = cookies == null ? returnedCookies : returnedCookies.Where(n => cookies.All(c => c.Name != n.Name)).ToArray();

                    //string newCookiesString = newCookies.Length == 0 ? null : string.Join(";", newCookies.Select(c => c.ToString()));

                    return new Response(response.StatusCode, data, text, response, newCookies, returnedCookies);
                }
                // ReSharper disable once UnusedVariable
                catch (HttpRequestException e)
                {
                    Console.WriteLine("[HTTP ERROR] {0}", e.Message);
                    /*if (e.Message.Contains("SSL"))
                    {
                        Console.WriteLine("            HResult: {0}", e.HResult);

                        if (retry == 0)
                        {
                            Console.WriteLine("Retrying...");
                            Thread.Sleep(250);
                            return DownloadData2(url, referrer,  cookies, 1);
                        }
                    }
                    else if (e.Message.Contains("Gateway Timeout"))
                    {
                        Console.WriteLine("            HResult: {0}", e.HResult);

                        if (retry == 0)
                        {
                            Console.WriteLine("Retrying...");
                            Thread.Sleep(30 * 1000);
                            return DownloadData2(url,  referrer, cookies, 1);
                        }
                    }
                    else if (e.Message.Contains("Origin Error"))
                    {
                        Console.WriteLine("            HResult: {0}", e.HResult);
                        Console.WriteLine("            URL:     {0}", url);
                    }
                    else*/
                    {
                        Console.WriteLine("        URL: {0}", url);
                    }

                    return new Response((HttpStatusCode)int.MaxValue, null, null, null, null, null);
                }
            }

        }

        // For compatibility with the WebClient version only.
        [Obsolete]
        public static byte[] DownloadData2(string url)
        {
            return DownloadData2(url, out var _);
        }

        // For compatibility with the WebClient version only.
        [Obsolete]
        public static byte[] DownloadData2(string url, out Error error)
        {
            var response = HttpDownload(url, null, null, 0);
            if (response.Status == HttpStatusCode.OK)
            {
                error = Error.Ok;
                return response.Data;
            }

            error = response.Status == HttpStatusCode.ServiceUnavailable ? Error.ServiceUnavailable : Error.Other;
            return null;
        }

#endregion
    }
}

