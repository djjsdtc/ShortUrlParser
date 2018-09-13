using System;
using System.IO;
using System.Net;

namespace ShortUrlParse
{
    /// <summary>
    /// 将短网址解析到其原始地址的小工具，附带识别一个短网址是不是 gbf 的功能。
    /// </summary>
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

        /// <summary>
        /// 解析短网址到其原始 URL，并指定解析深度。
        /// </summary>
        /// <param name="shortUrl">待解析的短网址。</param>
        /// <param name="maxParseTime">解析深度（循环次数）。如果给定小于等于 0 的值则表示仅对给定的 URL 进行解码处理。</param>
        /// <returns>解析出的原始 URL。如在给定深度内没有解析完成，则返回最后一次解析出的短网址。如解析失败则返回空字符串。</returns>
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

        /// <summary>
        /// 进行单次短网址解析。
        /// </summary>
        /// <param name="shortUrl">待解析的短网址。</param>
        /// <returns>单次解析结果。如有异常则抛出。</returns>
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

        /// <summary>
        /// 判断一个短网址是否为 GBF 的网址。
        /// </summary>
        /// <param name="shortUrl">待解析的短网址。</param>
        /// <param name="maxParseTime">解析深度（循环次数）。如果给定小于等于 0 的值则表示仅对给定的 URL 进行解码处理。</param>
        /// <returns>如原始地址为 GBF 地址则返回 true。如原始地址非 GBF 地址、解析失败或在给定的深度内无法解析出原始地址，则返回 false。</returns>
        public bool IsGranBlueFantasy(string shortUrl, int? maxParseTime = null) => ParseShortUrl(shortUrl, maxParseTime).Contains("granbluefantasy.jp");
    }
}
