using Infrastructure.Shared.Interfaces;
using System;
using System.Collections.Generic;
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



        public async Task<List<string>> FetchURLForHyperLinks(string url)
        {

            try
            {
                Thread.Sleep(100);
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                WebHeaderCollection header = response.Headers;

                var encoding = ASCIIEncoding.ASCII;
                string responseText = "";
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }

                List<string> hyperLinks = new List<string>();

                var linkParser = new Regex("<a [^>]*href=(?:'(?<href>.*?)')|(?:\"(?<href>.*?)\")", RegexOptions.IgnoreCase);
                var urlMatches = linkParser.Matches(responseText).OfType<Match>().Select(m => m.Groups["href"].Value);

                var urlList = urlMatches.Where(x => x.Length > 1 && !x.Contains(".") && (x.StartsWith("/") || x.StartsWith("#")) && Regex.IsMatch(x, "^.[A-Za-z]")).Distinct().ToList();

                return urlList;
            }
            catch (WebException e)
            {
                return null;
            }
        }
    }
}