using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface ITreeTraversalService
    {
        Task TraverseAsync(TreeNode treeNode);
        List<TreeNode> GetNodes();
    }
}
