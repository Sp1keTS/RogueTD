using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchTree : MonoBehaviour
{
    [System.Serializable]
    public class TreeSaveData
    {
        [System.Serializable]
        public class TreeSaveNode
        {
            public string nodeId;
            public string nodeAssetName; // Имя ScriptableObject ассета
            public int rank;
            public List<string> nextNodeIds = new List<string>();
            public bool isActive;
        }

        public List<TreeSaveNode> allNodes = new List<TreeSaveNode>();
        public List<string> rootNodeIds = new List<string>();
        
        private Dictionary<string, TreeSaveNode> _nodeCache;
        
        public TreeSaveNode GetNodeById(string id)
        {
            if (_nodeCache == null)
            {
                _nodeCache = allNodes.ToDictionary(n => n.nodeId, n => n);
            }
            return _nodeCache.TryGetValue(id, out var node) ? node : null;
        }
    }

    private List<TreeNode> roots = new List<TreeNode>();
    [SerializeField] private GameState gameState;
    private List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes = new Dictionary<TreeNode, float>();
    
    private int _maxRank = 9;
    [SerializeField] private int rootCount = 4;
    
    private Dictionary<string, TreeNode> _nodeAssetCache = new Dictionary<string, TreeNode>();
    private int _nodeCounter = 0;

    public void GenerateATree()
    {
        gameState.TreeSaveData = new TreeSaveData();
        _nodeCounter = 0;
        
        LoadAllNodes();
        ClearDependencies();
        CreateRoots();
        CreateBranches();
        UnloadAllNodes();
        
        Debug.Log($"Tree generated: {gameState.TreeSaveData.rootNodeIds.Count} roots, {gameState.TreeSaveData.allNodes.Count} total nodes");
    }

    private void LoadAllNodes()
    {
        TreeNode[] nodes = Resources.LoadAll<TreeNode>("Nodes");
        allAvailableNodes.AddRange(nodes);
        
        _nodeAssetCache.Clear();
        foreach (var node in allAvailableNodes)
        {
            _nodeAssetCache[node.name] = node;
        }
    }

    private void ClearDependencies()
    {
        foreach (TreeNode node in allAvailableNodes)
        {
            node.IsActive = false;
            node.PreviousNodes = null;
        }
    }

    private void CreateRoots()
    {
        _weightedNodes.Clear();
        
        foreach (TreeNode node in allAvailableNodes)
        {
            if (node.MinRank == 0 && node.Tags.Contains("Tower"))
            {
                _weightedNodes.Add(node, 1);
            }
        }

        roots.Clear();
        for (int i = 0; i < rootCount && _weightedNodes.Count > 0; i++)
        {
            TreeNode rootNode = GetRandomNode(_weightedNodes);
            if (rootNode != null)
            {
                rootNode.Initialize(0);
                roots.Add(rootNode);
                
                string nodeId = $"node_{_nodeCounter++}";
                gameState.TreeSaveData.rootNodeIds.Add(nodeId);
                AddOrGetSaveNode(rootNode, nodeId, 0);
                
                _weightedNodes.Remove(rootNode);
            }
        }
    }

    private void CreateBranches()
    {
        var currentRankNodeData = new List<(TreeNode node, string nodeId)>();
        // Добавляем корневые ноды с их ID
        foreach (var root in roots)
        {
            var saveNode = gameState.TreeSaveData.allNodes.FirstOrDefault(n => n.nodeAssetName == root.name);
            if (saveNode != null)
            {
                currentRankNodeData.Add((root, saveNode.nodeId));
            }
        }
        
        for (int currentRank = 1; currentRank <= _maxRank; currentRank++)
        {
            var nextRankNodeData = new List<(TreeNode node, string nodeId)>();
            var usedNodeTypesThisRank = new HashSet<string>();
            
            var availableNodesForRank = allAvailableNodes
                .Where(node => node.MinRank <= currentRank && node.MaxRank >= currentRank)
                .ToList();
                
            if (availableNodesForRank.Count == 0) 
                break;

            foreach (var (currentNode, currentNodeId) in currentRankNodeData)
            {
                int branchCount = Random.Range(1, 4);
                
                for (int i = 0; i < branchCount; i++)
                {
                    var possibleNodes = GetPossibleNodes(currentNode, currentRank, availableNodesForRank, usedNodeTypesThisRank);
                    
                    if (possibleNodes.Count == 0) 
                        break;
                    
                    var selectedNode = SelectNode(currentNode, possibleNodes, currentRank);
                    
                    if (selectedNode != null)
                    {
                        string newNodeId = $"node_{_nodeCounter++}";
                        
                        // Создаем связи с предыдущими нодами
                        var previousNodesList = new List<TreeNode>();
                        if (currentNode.PreviousNodes != null)
                            previousNodesList.AddRange(currentNode.PreviousNodes);
                        previousNodesList.Add(currentNode);
                        selectedNode.PreviousNodes = previousNodesList.ToArray();
                        
                        selectedNode.Initialize(currentRank);
                        
                        // Сохраняем связь в TreeSaveData
                        AddNodeConnection(currentNodeId, newNodeId);
                        AddOrGetSaveNode(selectedNode, newNodeId, currentRank);
                        
                        nextRankNodeData.Add((selectedNode, newNodeId));
                        usedNodeTypesThisRank.Add(selectedNode.name);
                        
                        if (selectedNode.Tags.Contains("Unique"))
                            allAvailableNodes.Remove(selectedNode);
                    }
                }
            }
            
            currentRankNodeData = nextRankNodeData;
            if (currentRankNodeData.Count == 0) 
                break;
        }
    }

    private void AddNodeConnection(string parentNodeId, string childNodeId)
    {
        var parentSaveNode = gameState.TreeSaveData.GetNodeById(parentNodeId);
        if (parentSaveNode != null && !parentSaveNode.nextNodeIds.Contains(childNodeId))
        {
            parentSaveNode.nextNodeIds.Add(childNodeId);
        }
    }

    private TreeSaveData.TreeSaveNode AddOrGetSaveNode(TreeNode node, string nodeId, int rank)
    {
        var saveNode = gameState.TreeSaveData.GetNodeById(nodeId);
        
        if (saveNode == null)
        {
            saveNode = new TreeSaveData.TreeSaveNode 
            { 
                nodeId = nodeId,
                nodeAssetName = node.name,
                rank = rank,
                isActive = node.IsActive
            };
            gameState.TreeSaveData.allNodes.Add(saveNode);
        }
        
        return saveNode;
    }

    // Получить TreeNode по save node
    private TreeNode GetTreeNodeBySaveNode(TreeSaveData.TreeSaveNode saveNode)
    {
        if (_nodeAssetCache.TryGetValue(saveNode.nodeAssetName, out var node))
        {
            // Восстанавливаем состояние ноды из save data
            node.IsActive = saveNode.isActive;
            node.CurrentRank = saveNode.rank;
            return node;
        }
        return null;
    }

    // Получить все дочерние ноды для конкретной ноды
    public List<TreeNode> GetChildren(TreeNode node)
    {
        if (gameState.TreeSaveData == null) return new List<TreeNode>();
        
        // Находим saveNode по имени ассета
        var saveNodes = gameState.TreeSaveData.allNodes
            .Where(n => n.nodeAssetName == node.name && n.isActive == node.IsActive && n.rank == node.CurrentRank)
            .ToList();
            
        var children = new List<TreeNode>();
        foreach (var saveNode in saveNodes)
        {
            foreach (var childId in saveNode.nextNodeIds)
            {
                var childSaveNode = gameState.TreeSaveData.GetNodeById(childId);
                if (childSaveNode != null)
                {
                    var childNode = GetTreeNodeBySaveNode(childSaveNode);
                    if (childNode != null)
                    {
                        children.Add(childNode);
                    }
                }
            }
        }
        return children.Distinct().ToList();
    }

    // Получить родительские ноды для конкретной ноды
    public List<TreeNode> GetParents(TreeNode node)
    {
        if (gameState.TreeSaveData == null) return new List<TreeNode>();
        
        var parents = new List<TreeNode>();
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            if (saveNode.nextNodeIds.Any(childId => 
            {
                var childSaveNode = gameState.TreeSaveData.GetNodeById(childId);
                return childSaveNode != null && childSaveNode.nodeAssetName == node.name;
            }))
            {
                var parentNode = GetTreeNodeBySaveNode(saveNode);
                if (parentNode != null)
                {
                    parents.Add(parentNode);
                }
            }
        }
        return parents.Distinct().ToList();
    }

    public bool CanActivateNode(TreeNode node)
    {
        if (node.IsActive) return false;
        
        var parents = GetParents(node);
        if (parents.Count == 0) return true;

        return parents.All(parent => parent.IsActive);
    }

    private Dictionary<TreeNode, float> GetPossibleNodes(TreeNode currentNode, int currentRank, List<TreeNode> availableNodes, HashSet<string> usedNodeTypes)
    {
        var weightedRandomNodes = new Dictionary<TreeNode, float>();
        
        foreach (var node in availableNodes)
        {
            if (usedNodeTypes.Contains(node.name))
                continue;
                
            float weight = CalculateRandomBranchWeight(currentNode, node, availableNodes);
            if (weight > 0)
            {
                weightedRandomNodes[node] = weight;
            }
        }
        
        return weightedRandomNodes;
    }

    private float CalculateRandomBranchWeight(TreeNode currentNode, TreeNode node, List<TreeNode> availableNodes)
    {
        float weight = 1;
        
        if (node.DirectUpgradeOf == currentNode) 
        { 
            weight += availableNodes.Count / 2; 
        }

        foreach (var tag in node.Tags)
        {
            if (currentNode.Tags.Contains(tag)) 
                weight += 2;
        }

        if (currentNode.PreviousNodes != null && currentNode.PreviousNodes.Length > 0)
        {
            var lastPreviousNode = currentNode.PreviousNodes[currentNode.PreviousNodes.Length - 1];
            if (lastPreviousNode.DirectUpgradeOf == node)
            {
                weight += availableNodes.Count / 4;
            }
        }

        return weight;
    }

    private TreeNode SelectNode(TreeNode currentNode, Dictionary<TreeNode, float> possibleNodes, int currentRank)
    {
        if (possibleNodes.Count == 0) return null;
        
        TreeNode node = GetRandomNode(possibleNodes);
        
        if (node is ProjectileTowerUpgradeTreeNode projectileTowerUpgradeNode)
        {
            var weightedProjectileTowerNodes = new Dictionary<TreeNode, float>();
            
            if (currentNode.PreviousNodes != null)
            {
                foreach (var prevNode in currentNode.PreviousNodes)
                {
                    if (prevNode is ProjectileTowerNode towerNode)
                    {
                        weightedProjectileTowerNodes[prevNode] = 1;
                    }
                }
            }
            
            TreeNode randomTowerNode = GetRandomNode(weightedProjectileTowerNodes);
            if (randomTowerNode is ProjectileTowerNode projectileTowerNode)
            {
                projectileTowerUpgradeNode.TowerToUpgrade = projectileTowerNode;
            }
        }
        
        return node;
    }

    public static TreeNode GetRandomNode(Dictionary<TreeNode, float> weightedNodes)
    {
        if (weightedNodes == null || weightedNodes.Count == 0)
            return null;

        float totalWeight = weightedNodes.Values.Sum();
        if (totalWeight <= 0)
            return null;

        float randomValue = Random.Range(0f, totalWeight);
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
        roots.Clear();
        _weightedNodes.Clear();
        _nodeAssetCache.Clear();
    }
}