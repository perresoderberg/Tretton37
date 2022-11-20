using Infrastructure.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Shared
{
    public class HTTPClientService : IHTTPClientService
    {
        private static object _lock = new object();

        private readonly ILogger<HTTPClientService> _logger;
        public HTTPClientService(ILogger<HTTPClientService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<string>> FetchURLForHyperLinksAsync(string url)
        {

            try
            {
                _logger.LogTrace($"Fetch from URL: {url}");

                Thread.Sleep(5);

                List<string> urlList = null;
                lock (_lock)
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    WebHeaderCollection header = response.Headers;

                    string responseStr = null;
                    using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        responseStr = reader.ReadToEnd();
                    }

                    urlList = RetreiveHyperLinksFromHtml(responseStr);
                }
                _logger.LogTrace("Found URLs: " + string.Join(',', urlList));

                return urlList;
            }
            catch (WebException ex)
            {
                _logger.LogError("Exception in FetchURLForHyperLinksAsync");
                Debugger.Break();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in FetchURLForHyperLinksAsync");
                Debugger.Break();
                throw;
            }
        }

        public List<string> RetreiveHyperLinksFromHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                throw new ArgumentNullException("In method RetreiveHyperLinksFromHtml, the provided string is empty");

            var linkParser = new Regex("<a [^>]*href=(?:'(?<href>.*?)')|(?:\"(?<href>.*?)\")", RegexOptions.IgnoreCase);
            var urlMatches = linkParser.Matches(html).OfType<Match>().Select(m => m.Groups["href"].Value);
            return urlMatches.Where(x => x.Length > 1 && !x.Contains(".") && (x.StartsWith("/") || x.StartsWith("#")) && Regex.IsMatch(x, "^.[A-Za-z]")).Distinct().ToList();
        }

        public async Task<string> GetHtmlPageAsync(string url) 
        {

            string htmlPage = "";
            
            HttpWebResponse response = null;
            StreamReader reader = null;

            try 
            {
                lock (_lock)
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                    response = (HttpWebResponse)request.GetResponse();

                    WebHeaderCollection header = response.Headers;

                    var encoding = ASCIIEncoding.ASCII;
                    using (reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        htmlPage = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                _logger.LogError("WebException in GetHtmlPageAsync");
                Debugger.Break();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("WebException in GetHtmlPageAsync");
                Debugger.Break();
                throw;
            }
            finally 
            {
                if (response != null)
                    response.Dispose();
                if (reader != null)
                    reader.Dispose();
            }
            return htmlPage;
        }

    }
}