using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{

    public class TreeNodeBase
    {
        public string BaseUrl { get; init; }
        public List<string> UsedUrls { get; set; }
        public TreeNodeBase(string baseUrl, List<string> usedUrls)
        {
            BaseUrl = baseUrl;
            UsedUrls = usedUrls;
        }
    }

    public class TreeNode : TreeNodeBase
    {
        public int Level { get; set; }
        public string CurrentUrl { get; set; }

        public TreeNode(string baseUrl, string currentUrl, List<string> usedUrls) : base(baseUrl, usedUrls)
        {
            CurrentUrl = currentUrl;
        }

    }
}
