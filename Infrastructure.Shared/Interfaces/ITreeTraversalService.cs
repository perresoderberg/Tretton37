﻿using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Interfaces
{
    public  interface ITreeTraversalService
    {
        Task TraverseAsync(List<TreeNode> treeNodes, string baseUrl, string currentUrl, List<string> usedUrls);
    }
}
