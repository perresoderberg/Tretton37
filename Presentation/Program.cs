using Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tretton37
{

    public class TreeNode
    {
        public string Url { get; set; }
        public int Level { get; set; }
    }


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
            HTTPClientService c = new HTTPClientService();

            List<TreeNode> treeNodes = new List<TreeNode>();

            List<string> hyperLinks = await c.FetchURLForHyperLinks(baseUrl);

            List<string> usedUrls = new List<string>();

            usedUrls.AddRange(hyperLinks);

            foreach (string link in hyperLinks)
            {
                //TODO
                // traverse the url in some way

            }

        }
    }
}
