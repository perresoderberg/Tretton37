using Core.Domain;
using Infrastructure.Shared;
using Infrastructure.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application
{
    public class TreeTraversalService : ITreeTraversalService
    {
        IOService ioService;
        HTTPClientService httpClientService;
        public TreeTraversalService()
        {
            ioService = new IOService();
            httpClientService = new HTTPClientService();
        }

        public async Task Traverse(List<TreeNode> treeNodes, string baseUrl, string currentUrl, List<string> usedUrls)
        {
            try
            {

                if (string.IsNullOrEmpty(currentUrl))
                    return;

                int level = currentUrl.Count(x => x == '/' || x == '#');
                treeNodes.Add(new TreeNode() { Level = level, Url = currentUrl });

                List<string> hyperlinks = await httpClientService.FetchURLForHyperLinks(currentUrl);

                if (hyperlinks.Count == 0)
                    return;


                hyperlinks.RemoveAll(x => usedUrls.Contains(x));

                foreach (var link in hyperlinks)
                {
                    if (link.Contains("meet") && treeNodes.Count > 30)
                        continue;

                    if (link.Contains("vacancies"))
                        ;
                    if (treeNodes.Count == 30)
                        ;

                    if (treeNodes.Count == 50)
                        ;

                    string newLink = currentUrl + link;




                    if (link.StartsWith("/"))
                    {
                        newLink = baseUrl + link;
                    }
                    if (usedUrls.Contains(link))
                        continue;

                    usedUrls.Add(link);


                    await Traverse(treeNodes, baseUrl, newLink, usedUrls);
                }

            }
            catch (Exception ex)
            {

                throw;
            }


        }

    }
}
