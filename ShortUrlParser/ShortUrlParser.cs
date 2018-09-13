using System;
using System.IO;
using System.Net;

namespace ShortUrlParse
{
    public class ShortUrlParser
    {
        protected class WebClientForShortUrl : WebClient
        {
            private HttpWebRequest request = null;

            protected override WebRequest GetWebRequest(Uri address)
            {
                request = (HttpWebRequest)base.GetWebRequest(address);
                request.AllowAutoRedirect = false;
                return request;
            }

            public HttpStatusCode GetStatusCode()
            {
                HttpStatusCode result;

                if (request == null)
                {
                    throw new InvalidOperationException("Unable to retrieve the status code.");
                }

                HttpWebResponse response = base.GetWebResponse(request) as HttpWebResponse;

                if (response != null)
                {
                    result = response.StatusCode;
                }
                else
                {
                    throw new InvalidOperationException("Unable to retrieve the status code.");
                }

                return result;
            }
        }

        public string ParseShortUrl(string shortUrl, int? maxParseTime = null)
        {
            shortUrl = WebUtility.UrlDecode(shortUrl);

            if (maxParseTime != null && maxParseTime <= 0)
            {
                return shortUrl;
            }

            int parsedTimes = 0;
            try
            {
                while(true)
                {
                    string parsedUrl = InnerParseShortUrl(shortUrl);
                    if ((maxParseTime != null && ++parsedTimes == maxParseTime) || parsedUrl == shortUrl)
                    {
                        return parsedUrl;
                    }
                    else
                    {
                        shortUrl = parsedUrl;
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        protected string InnerParseShortUrl(string shortUrl)
        {
            shortUrl = WebUtility.UrlDecode(shortUrl);
            using (WebClientForShortUrl client = new WebClientForShortUrl())
            {
                client.Headers.Add("Referer", shortUrl);
                Stream stream = null;
                try
                {
                    stream = client.OpenRead(shortUrl);
                    int statusCode = (int)client.GetStatusCode();
                    if (statusCode >= 300 && statusCode <= 399)
                    {
                        return client.ResponseHeaders["Location"];
                    }
                    else
                    {
                        return shortUrl;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (stream != null) stream.Close();
                }
            }
        }

        public bool IsGranBlueFantasy(string shortUrl, int? maxParseTime = null) => ParseShortUrl(shortUrl, maxParseTime).Contains("granbluefantasy.jp");
    }
}
