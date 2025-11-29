using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ResearchTree : MonoBehaviour
{
    [SerializeField] private int _maxRank = 7;
    [SerializeField] private int rootCount = 4;
    [SerializeField] GameState gameState;
    
    static List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes = new Dictionary<TreeNode, float>();
    
    public struct TreeSaveData 
    {
        public class TreeSaveNode
        {
            public TreeSaveNode(TreeNode node, List<TreeSaveNode> nextNodes, List<TreeSaveNode> visited)
            {
                currentNode = node;
                nextSaveNodes = nextNodes ?? new List<TreeSaveNode>();
                visitedNodes = visited ?? new List<TreeSaveNode>();
            }

            public List<TreeSaveNode> nextSaveNodes;
            public List<TreeSaveNode> visitedNodes;
            public TreeNode currentNode;
        }

        public static List<TreeSaveNode> rootSaveNodes = new List<TreeSaveNode>();
    }

    public void GenerateATree()
    {
        try
        {
            gameState.TreeSaveData = new ResearchTree.TreeSaveData();
            ClearDependencies();
            LoadAllNodes();
            
            if (allAvailableNodes.Count == 0)
            {
                Debug.LogError("No nodes loaded from Resources!");
                return;
            }
            
            CreateRoots();
            
            if (TreeSaveData.rootSaveNodes.Count == 0)
            {
                Debug.LogError("No root nodes created!");
                return;
            }
            
            CreateBranches();
            
            UnloadAllNodes();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating research tree: {e.Message}\n{e.StackTrace}");
        }
    }

    private void CreateRoots()
    {
        TreeSaveData.rootSaveNodes.Clear();
        _weightedNodes.Clear();
        
        foreach (TreeNode node in allAvailableNodes)
        {
            if (node.Tags?.Contains("Tower") == true && node.MinRank == 0)
            {
                _weightedNodes[node] = 1;
            }
        }
        
        for (int i = 0; i < rootCount && _weightedNodes.Count > 0; i++)
        {
            var rootNode = GetRandomNode(_weightedNodes);
            if (rootNode != null)
            {
                rootNode.Initialize(0);
                var rootSaveNode = new TreeSaveData.TreeSaveNode(
                    rootNode, 
                    new List<TreeSaveData.TreeSaveNode>(), 
                    new List<TreeSaveData.TreeSaveNode>()
                );
                TreeSaveData.rootSaveNodes.Add(rootSaveNode);
                
                _weightedNodes.Remove(rootNode);
            }
        }
    }

    public void CreateBranches()
    {
        var currentRank = TreeSaveData.rootSaveNodes;
        var nextRank = new List<TreeSaveData.TreeSaveNode>();
        
        for (int rank = 1; rank < _maxRank; rank++)
        {
            var currentRankCopy = new List<TreeSaveData.TreeSaveNode>(currentRank);
            
            foreach (var node in currentRankCopy)
            {
                // Основная линия - всегда продолжается
                CreateBranchNode(node, nextRank, rank);
                
                // Дополнительные ветви - создаются с вероятностью
                bool shouldCreateSideBranch = UnityEngine.Random.value < GetSideBranchProbability(rank);
                if (shouldCreateSideBranch)
                {
                    CreateBranchNode(node, nextRank, rank);
                }
            }
            
            currentRank = nextRank;
            nextRank = new List<TreeSaveData.TreeSaveNode>();
        }
    }

    private void CreateBranchNode(TreeSaveData.TreeSaveNode parentNode, List<TreeSaveData.TreeSaveNode> nextRank, int rank)
    {
        var visited = new List<TreeSaveData.TreeSaveNode>(parentNode.visitedNodes ?? new List<TreeSaveData.TreeSaveNode>());
        visited.Add(parentNode);
        
        var nextNode = new TreeSaveData.TreeSaveNode(null, new List<TreeSaveData.TreeSaveNode>(), visited);
        
        parentNode.nextSaveNodes = parentNode.nextSaveNodes ?? new List<TreeSaveData.TreeSaveNode>();
        parentNode.nextSaveNodes.Add(nextNode);
        nextRank.Add(nextNode);
        
        _weightedNodes.Clear();
        foreach (var treeNode in allAvailableNodes)
        {
            var tempNode = new TreeSaveData.TreeSaveNode(treeNode, new List<TreeSaveData.TreeSaveNode>(), visited);
            float weight = CalculateRandomBranchWeight(tempNode, parentNode, allAvailableNodes);
            _weightedNodes[treeNode] = weight;
        }
        
        var selectedNode = GetRandomNode(_weightedNodes);
        if (selectedNode != null)
        {
            if (selectedNode.CurrentRank == 0)
            {
                selectedNode.CurrentRank = rank;
                selectedNode.Initialize(rank);
            }
            nextNode.currentNode = selectedNode;
            
            if (selectedNode is ProjectileTowerUpgradeTreeNode projectileNode)
            {
                ProcessUpgradeNode(projectileNode, visited);
            }
        }
    }

    private float GetSideBranchProbability(int rank)
    {
        switch (rank)
        {
            case 1: return 0.50f;
            case 2: return 0.40f;
            case 3: return 0.30f;
            case 4: return 0.20f;
            case 5: return 0.15f;
            case 6: return 0.1f;
            default: return 0.05f;
        }
    }

    private void ProcessUpgradeNode(ProjectileTowerUpgradeTreeNode projectileNode, List<TreeSaveData.TreeSaveNode> visited)
    {
        projectileNode.TowersToUpgrade = projectileNode.TowersToUpgrade ?? new List<ProjectileTowerNode>();
        
        var towerCandidates = new Dictionary<ProjectileTowerNode, float>();
        foreach (var saveNode in visited)
        {
            if (saveNode?.currentNode is ProjectileTowerNode towerNode &&
                towerNode.Tags?.Contains("Tower") == true &&
                !projectileNode.TowersToUpgrade.Contains(towerNode))
            {
                towerCandidates[towerNode] = 1;
            }
        }
        
        // Добавляем одну случайную башню из доступных кандидатов
        if (towerCandidates.Count > 0)
        {
            var randomTower = GetRandomTowerNode(towerCandidates);
            if (randomTower != null)
            {
                projectileNode.TowersToUpgrade.Add(randomTower);
            }
        }
    }

    private ProjectileTowerNode GetRandomTowerNode(Dictionary<ProjectileTowerNode, float> weightedNodes)
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

    private float CalculateRandomBranchWeight(TreeSaveData.TreeSaveNode currentNode, TreeSaveData.TreeSaveNode prevNode, List<TreeNode> availableNodes)
    {
        if (prevNode?.currentNode == null || currentNode?.currentNode == null)
            return 1f;

        var node = prevNode.currentNode;
        var cNode = currentNode.currentNode;
        float weight = 1;
        
        if (node.DirectUpgradeOf == cNode) 
        { 
            weight += (float)availableNodes.Count / 2; 
        }

        if (node.Tags != null && cNode.Tags != null)
        {
            foreach (var tag in node.Tags)
            {
                if (cNode.Tags.Contains(tag)) 
                    weight += 2;
            }
        }

        if (currentNode.visitedNodes != null && currentNode.visitedNodes.Count > 0)
        {
            var lastPreviousNode = currentNode.visitedNodes[0];
            if (lastPreviousNode?.currentNode?.DirectUpgradeOf == node)
            {
                weight += (float)availableNodes.Count / 4;
            }
        }

        return weight;
    }
    
    private void LoadAllNodes()
    {
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

        var nodeToReturn = weightedNodes.Keys.Last();
        if (nodeToReturn.Tags?.Contains("Unique") == true) {allAvailableNodes.Remove(nodeToReturn);}
        return nodeToReturn;
    }
     
    private void UnloadAllNodes()
    {
        allAvailableNodes.Clear();
    }
}