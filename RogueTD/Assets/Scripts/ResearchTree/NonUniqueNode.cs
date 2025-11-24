using System.Collections.Generic;
using UnityEngine;

public abstract class NonUniqueNode : TreeNode
{
    public static Dictionary<int, TreeNode> NodeOccurrences = new Dictionary<int, TreeNode>();
}
