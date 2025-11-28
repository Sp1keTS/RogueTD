using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ResearchTree : MonoBehaviour
{
     
    private TreeNode[] roots = new TreeNode[0];
    [SerializeField] GameState gameState;
    public TreeNode[] Roots => roots;
    private List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes;

    private int _maxRank = 9;
    [SerializeField] private int rootCount = 4;
    public struct TreeSaveData 
    {
        private class TreeSaveNode
        {
            List<TreeSaveNode> nextSaveNodes = new List<TreeSaveNode>();
            TreeNode currentNode;
        }

        private List<TreeSaveNode> rootSaveNodes;
    }
    public void GenerateATree()
    {
        gameState.TreeSaveData = new ResearchTree.TreeSaveData();
        LoadAllNodes();
        ClearDependencies();
        CreateRoots();
        CreateBranches();
        UnloadAllNodes();
    }
    
     private void LoadAllNodes()
     {
         // загрузка всех нод из папки Resources/Nodes
         TreeNode[] nodes = Resources.LoadAll<TreeNode>("Nodes");
         allAvailableNodes.AddRange(nodes);
     }

     private void ClearDependencies()
     {
         foreach (TreeNode node in allAvailableNodes)
         {
             node.IsActive = false;
         }
     }
    
     private void CreateRoots()
     {
         foreach (TreeNode node in allAvailableNodes)
         {
             if (node.MinRank == 0 && node.Tags.Contains("Tower"))
             {
                 _weightedNodes.Add(node, 1);
             }
         }
         for (int i = 0; i < rootCount; i++)
         {
             Roots.Append(GetRandomNode(_weightedNodes));
         }
         _weightedNodes.Clear();
     }

     private void CreateBranches()
     {
         var currentRankNodes = new List<TreeNode>(roots);
         for (int currentRank = 1; currentRank <= _maxRank; currentRank++)
         {
             var nextRankNodes = new List<TreeNode>();
             _weightedNodes.Clear();

             foreach (var currentNode in currentRankNodes)
             {
                 for (int i = 1; i <= Random.Range(0, 3); i++)
                 {
                     var possibleNodes = GetPossibleNodes(currentNode, currentRank);
                     var selectedNode = SelectNode(currentNode, possibleNodes, currentRank);
                     if (selectedNode != null)
                     {
                         selectedNode.PreviousNodes = (TreeNode[])currentNode.PreviousNodes.Append(currentNode);
                         nextRankNodes.Add(selectedNode);

                     }
                 }
             }
         }
     }

     private TreeNode SelectNode(TreeNode currentNode, Dictionary<TreeNode, float> possibleNodes, int currentRank)
     {
         TreeNode node = GetRandomNode(possibleNodes);
         ProjectileTowerUpgradeTreeNode upgradeNode = null;
         if (node.GetType() == typeof(ProjectileTowerUpgradeTreeNode))
         {
             upgradeNode = (ProjectileTowerUpgradeTreeNode)node;
             var weightedTowerNodes =  new Dictionary<TreeNode, float>();
             foreach (var towerNode in currentNode.PreviousNodes)
             {
                 if (towerNode.Tags.Contains("Tower"))
                 {
                     weightedTowerNodes[towerNode] = 1;
                 }
             }
             upgradeNode.TowerToUpgrade = GetProjectileTowerNode(weightedTowerNodes);
         }
         
         if (upgradeNode)
         {
             if(upgradeNode.Tags.Contains("Unique")) allAvailableNodes.Remove(upgradeNode);
             upgradeNode.Initialize(currentRank);
             return upgradeNode;
         }
         if(node.Tags.Contains("Unique")) allAvailableNodes.Remove(node);
         node.Initialize(currentRank);
         return node;
     }

     private ProjectileTowerNode GetProjectileTowerNode(Dictionary<TreeNode, float> weightedTowerNodes)
     {
         var selectedNode = GetRandomNode(weightedTowerNodes);
    
         if (selectedNode is ProjectileTowerNode projectileTowerNode)
         {
             return projectileTowerNode;
         }
    
         return null;
     }

     private Dictionary<TreeNode, float>  GetPossibleNodes(TreeNode currentNode, int currentRank)
     {
         var availableNodes = allAvailableNodes.Where(node => 
             node.MinRank <= currentRank && 
             node.MaxRank >= currentRank
         ).ToList();
         
         var weightedRandomNodes = new Dictionary<TreeNode, float>();
         foreach (var node in availableNodes)
         {
             float weight = CalculateRandomBranchWeight(currentNode, node, availableNodes);
             weightedRandomNodes[node] = weight;
         }
         
         return weightedRandomNodes;
     }

     private float CalculateRandomBranchWeight(TreeNode currentNode, TreeNode node, List<TreeNode> availableNodes)
     {
         var weight = 1;
         if (node.DirectUpgradeOf == currentNode) {weight += availableNodes.Count/2; }

         foreach (var tag in node.Tags)
         {
             if (currentNode.Tags.Contains(tag)) weight += 2;
         }

         if (currentNode.PreviousNodes != null)
         {
             if (currentNode.PreviousNodes[-1].DirectUpgradeOf == node)
             {
                 weight += availableNodes.Count / 4;
             }
         }

         return weight;
     }

     public static TreeNode GetRandomNode(Dictionary<TreeNode, float> weightedNodes)
     {
         if (weightedNodes == null || weightedNodes.Count == 0)
             return null;

         float totalWeight = weightedNodes.Values.Sum();
    
         if (totalWeight <= 0)
             return null;

         float randomValue = UnityEngine.Random.Range(0f, totalWeight);
         float accumulatedWeight = 0f;

         foreach (var kvp in weightedNodes)
         {
             accumulatedWeight += kvp.Value;
             if (randomValue < accumulatedWeight)
             {
                 return kvp.Key;
             }
         }

         return weightedNodes.Keys.Last();
     }
     
     private void UnloadAllNodes()
     {
         allAvailableNodes.Clear();
     }
     
}
