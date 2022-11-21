using Application;
using Core.Domain;
using Infrastructure.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application
{
    public class TreeTraversalService : ITreeTraversalService
    {
        public static volatile int numbersOfEntries = 0;
        public static volatile int numbersAwaitedThreads = 0;

        IIOService _ioService;
        IHTTPClientService _httpClientService;

        private readonly ILogger<TreeTraversalService> _logger;

        public TreeTraversalService(IIOService ioService, IHTTPClientService httpClientService, ILogger<TreeTraversalService> logger)
        {
            _ioService = ioService;
            _httpClientService = httpClientService;
            _logger = logger;
        }
        /// <summary>
        /// Designed to be traversed recursively. Traverse a web page to retreive links and from them traverse futher.
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="baseUrl"></param>
        /// <param name="currentUrl"></param>
        /// <param name="usedUrls"></param>
        /// <returns></returns>
        public async Task TraverseAsync(List<TreeNode> treeNodes, string baseUrl, string currentUrl, List<string> usedUrls)
        {
            try
            {
                _logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId}  Enter Traverse with Url {currentUrl}");

                if (string.IsNullOrEmpty(currentUrl))
                    return;

                int level = currentUrl.Count(x => x == '/' || x == '#');
                treeNodes.Add(new TreeNode() { Level = level, Url = currentUrl });

                var html = await _httpClientService.GetHtmlPageAsync(currentUrl);

                await _ioService.StoreHtmlPageInFilePathAsync(currentUrl.Replace(baseUrl, ""), html);

                numbersOfEntries++;

                var hyperlinks = _httpClientService.RetreiveHyperLinksFromHtml(html);

                if (hyperlinks.Count == 0)
                    return;

                hyperlinks.RemoveAll(x => usedUrls.Contains(x));

                if (hyperlinks.Count == 0)
                    return;

                _logger.LogTrace("  all found hyperlinks - " + String.Join(',', hyperlinks)); ;

                var tasks = new List<Task>();

                //foreach (var link in hyperlinks)
                //{
                //    string newLink = currentUrl + link;
                //    if (link.StartsWith("/"))
                //    {
                //        newLink = baseUrl + link;
                //    }
                //    if (!usedUrls.Contains(link))
                //    {
                //        usedUrls.Add(link);

                //        var t = TraverseAsync(treeNodes, baseUrl, newLink, usedUrls);

                //        tasks.Add(t);
                //    }
                //}

                //** An other approach to handle threads **//

                Parallel.ForEach(hyperlinks, link =>
                {
                    var newlink = currentUrl + link;
                    if (link.StartsWith("/"))
                    {
                        newlink = baseUrl + link;
                    }
                    if (!usedUrls.Contains(link))
                    {
                        usedUrls.Add(link);
                        var t = TraverseAsync(treeNodes, baseUrl, newlink, usedUrls);
                        tasks.Add(t);
                    }
                });

                await Task.WhenAll(tasks);
                numbersAwaitedThreads += tasks.Count;

                _logger.LogInformation($"wait for all {tasks.Count} tasks to complete");

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in TraverseAsync");
                Debugger.Break();
                throw;
            }


        }

    }
}
