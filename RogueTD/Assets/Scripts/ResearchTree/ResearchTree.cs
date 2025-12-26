using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class ResearchTree : MonoBehaviour
{
    [SerializeField] static public int _maxRank = 7;
    [SerializeField] private int rootCount = 4;
    
    static List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes = new Dictionary<TreeNode, float>();
    private HashSet<TreeNode> _usedUniqueNodes = new HashSet<TreeNode>();

    [System.Serializable]
    public class TreeSaveData
    {
        [System.Serializable]
        public class TreeSaveNode
        {
            public string currentNodeId;
            public string nodeToUpgradeId;
            public bool IsActive;
            public List<TreeSaveNode> nextSaveNodes;
            public List<TreeSaveNode> visitedNodes;
        
            public TreeSaveNode(TreeNode node, List<TreeSaveNode> nextNodes, List<TreeSaveNode> visited)
            {
                currentNodeId = node?.name ?? string.Empty;
                nodeToUpgradeId = string.Empty;
                IsActive = false;
                nextSaveNodes = nextNodes ?? new List<TreeSaveNode>();
                visitedNodes = visited ?? new List<TreeSaveNode>();
            }
        
            public TreeSaveNode() 
            {
                nextSaveNodes = new List<TreeSaveNode>();
                visitedNodes = new List<TreeSaveNode>();
            }
        
            [JsonIgnore]
            public TreeNode currentNode
            {
                get
                {
                    if (string.IsNullOrEmpty(currentNodeId))
                        return null;
                    
                    return ResourceManager.GetTreeNode(currentNodeId);
                }
                set
                {
                    currentNodeId = value?.name ?? string.Empty;
                }
            }
        
            [JsonIgnore]
            public ProjectileTowerNode nodeToUpgrade
            {
                get
                {
                    if (string.IsNullOrEmpty(nodeToUpgradeId))
                        return null;
                    
                    return ResourceManager.GetTreeNode(nodeToUpgradeId) as ProjectileTowerNode;
                }
                set
                {
                    nodeToUpgradeId = value?.name ?? string.Empty;
                }
            }
        }

        public List<TreeSaveNode> rootSaveNodes;
    }

    public void GenerateATree()
    {
        try
        {
            GameState.Instance.TreeSaveData = new TreeSaveData
            {
                rootSaveNodes = new List<TreeSaveData.TreeSaveNode>()
            };
            
            ClearDependencies();
            LoadAllNodes();
            
            if (allAvailableNodes.Count == 0)
            {
                Debug.LogError("No nodes loaded from Resources!");
                return;
            }
            
            CreateRoots();
            
            if (GameState.Instance.TreeSaveData.rootSaveNodes == null || GameState.Instance.TreeSaveData.rootSaveNodes.Count == 0)
            {
                Debug.LogError("No root nodes created!");
                return;
            }
            
            CreateBranches();
            
            UnloadAllNodes();
            GameState.Instance.SaveResearchTree();
            Debug.Log($"Tree generated successfully with {GameState.Instance.TreeSaveData.rootSaveNodes.Count} root nodes");
            
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating research tree: {e.Message}\n{e.StackTrace}");
        }
    }

    private void CreateRoots()
    {
        GameState.Instance.TreeSaveData.rootSaveNodes.Clear();
        _weightedNodes.Clear();
        
        foreach (TreeNode node in allAvailableNodes)
        {
            if (node.Tags?.Contains("Tower") == true && 
                node.MinRank == 0 &&
                !IsUniqueNodeUsed(node))
            {
                _weightedNodes[node] = 1;
            }
        }
        
        Debug.Log($"Found {_weightedNodes.Count} potential root nodes");
        
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
                
                GameState.Instance.TreeSaveData.rootSaveNodes.Add(rootSaveNode);
                _weightedNodes.Remove(rootNode);
                
                Debug.Log($"Added root node: {rootNode.name}");
            }
        }
    }

    public void CreateBranches()
    {
        var currentRank = GameState.Instance.TreeSaveData.rootSaveNodes;
        var nextRank = new List<TreeSaveData.TreeSaveNode>();
        
        for (int rank = 1; rank < _maxRank; rank++)
        {
            var currentRankCopy = new List<TreeSaveData.TreeSaveNode>(currentRank);
            
            foreach (var node in currentRankCopy)
            {
                CreateBranchNode(node, nextRank, rank);
                
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
            if (!IsUniqueNodeUsed(treeNode))
            {
                var tempNode = new TreeSaveData.TreeSaveNode(treeNode, new List<TreeSaveData.TreeSaveNode>(), visited);
                float weight = CalculateRandomBranchWeight(tempNode, parentNode, allAvailableNodes);
                _weightedNodes[treeNode] = weight;
            }
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
                var towerToUpgrade = GetTowerToUpgradeForNode(parentNode, visited);
                nextNode.nodeToUpgrade = towerToUpgrade;
            
            }
        }
    }

    private ProjectileTowerNode GetTowerToUpgradeForNode(TreeSaveData.TreeSaveNode parentNode, List<TreeSaveData.TreeSaveNode> visited)
    {
        var towerCandidates = new Dictionary<ProjectileTowerNode, float>();
    
        foreach (var saveNode in visited)
        {
            if (saveNode?.currentNode is ProjectileTowerNode towerNode)
            {
                if (!towerCandidates.ContainsKey(towerNode))
                {
                    towerCandidates[towerNode] = 1f; 
                }
            }
        }
    
        if (parentNode?.currentNode is ProjectileTowerNode parentTower)
        {
            if (!towerCandidates.ContainsKey(parentTower))
            {
                towerCandidates[parentTower] = 1f;
            }
        }
    
        if (towerCandidates.Count > 0)
        {
            return GetRandomTowerNode(towerCandidates);
        }
    
        return null;
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
        if (!prevNode?.currentNode || !currentNode?.currentNode)
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
        allAvailableNodes.RemoveAll(node => IsUniqueNodeUsed(node));
        Debug.Log($"Loaded {allAvailableNodes.Count} nodes from Resources");
    }

    private void ClearDependencies()
    {
        // Очищаем только состояние TreeNode
        foreach (TreeNode node in allAvailableNodes)
        {
            // IsActive теперь управляется через TreeSaveNode
        }
    }
    
    private bool IsUniqueNodeUsed(TreeNode node)
    {
        return node.Tags?.Contains("Unique") == true && _usedUniqueNodes.Contains(node);
    }
     
    public TreeNode GetRandomNode(Dictionary<TreeNode, float> weightedNodes)
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
                var selectedNode = kvp.Key;
                if (selectedNode.Tags?.Contains("Unique") == true)
                {
                    _usedUniqueNodes.Add(selectedNode);
                    allAvailableNodes.Remove(selectedNode);
                    Debug.Log($"Unique node marked as used: {selectedNode.name}");
                }
                return selectedNode;
            }
        }

        var nodeToReturn = weightedNodes.Keys.Last();
        if (nodeToReturn.Tags?.Contains("Unique") == true)
        {
            _usedUniqueNodes.Add(nodeToReturn);
            allAvailableNodes.Remove(nodeToReturn);
            Debug.Log($"Unique node marked as used: {nodeToReturn.name}");
        }
        return nodeToReturn;
    }
     
    private void UnloadAllNodes()
    {
        allAvailableNodes.Clear();
        _usedUniqueNodes.Clear(); 
    }
}