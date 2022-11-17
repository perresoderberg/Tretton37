using Core.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Shared.Interfaces;
using Core.Domain;

namespace Tretton37
{

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Task.WaitAll(MainAsync());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        public static async Task MainAsync()
        {
            string baseUrl = @"http://tretton37.com";
            
            ITreeTraversalService _treeTraversalService = new TreeTraversalService();

            List<TreeNode> treeNodes = new List<TreeNode>();
            List<string> usedUrls = new List<string>();

            await _treeTraversalService.Traverse(treeNodes, baseUrl, baseUrl, usedUrls);

            //List<string> hyperLinks = await _treeTraversalService.FetchURLForHyperLinks(baseUrl);

            //usedUrls.AddRange(hyperLinks);

            //foreach (string link in hyperLinks)
            //{
            //    await c.Traverse(treeNodes, baseUrl, baseUrl + link, usedUrls);
            //}

        }
    }
}
