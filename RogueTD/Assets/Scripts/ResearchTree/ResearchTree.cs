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
            public TreeNode currentNode;
            public List<TreeNode> nextNodes = new List<TreeNode>();
        }

        public List<TreeSaveNode> allNodes = new List<TreeSaveNode>();
        public List<TreeNode> rootNodes = new List<TreeNode>();
    }

    private List<TreeNode> roots = new List<TreeNode>();
    [SerializeField] private GameState gameState;
    private List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes = new Dictionary<TreeNode, float>();
    
    private int _maxRank = 9;
    [SerializeField] private int rootCount = 4;

    public void GenerateATree()
    {
        gameState.TreeSaveData = new TreeSaveData();
        
        LoadAllNodes();
        ClearDependencies();
        CreateRoots();
        CreateBranches();
        UnloadAllNodes();
        
        Debug.Log($"Tree generated: {gameState.TreeSaveData.rootNodes.Count} roots, {gameState.TreeSaveData.allNodes.Count} total nodes");
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
            node.PreviousNodes = null;
        }
    }

    private void CreateRoots()
    {
        _weightedNodes.Clear();
        
        // Собираем все корневые ноды
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
            TreeNode root = GetRandomNode(_weightedNodes);
            if (root != null)
            {
                root.Initialize(0);
                roots.Add(root);
                
                // Сразу добавляем корневую ноду в сохранение
                gameState.TreeSaveData.rootNodes.Add(root);
                AddOrGetSaveNode(root);
                
                _weightedNodes.Remove(root);
            }
        }
    }

    private void CreateBranches()
    {
        var currentRankNodes = new List<TreeNode>(roots);
        
        for (int currentRank = 1; currentRank <= _maxRank; currentRank++)
        {
            var nextRankNodes = new List<TreeNode>();
            
            foreach (var currentNode in currentRankNodes)
            {
                int branchCount = Random.Range(1, 4); // 1-3 ветки от текущей ноды
                
                for (int i = 0; i < branchCount; i++)
                {
                    var possibleNodes = GetPossibleNodes(currentNode, currentRank);
                    var selectedNode = SelectNode(currentNode, possibleNodes, currentRank);
                    
                    if (selectedNode != null)
                    {
                        // Создаем связи с предыдущими нодами
                        var previousNodesList = new List<TreeNode>();
                        if (currentNode.PreviousNodes != null)
                            previousNodesList.AddRange(currentNode.PreviousNodes);
                        previousNodesList.Add(currentNode);
                        selectedNode.PreviousNodes = previousNodesList.ToArray();
                        
                        // Сохраняем связь в TreeSaveData
                        AddNodeConnection(currentNode, selectedNode);
                        
                        nextRankNodes.Add(selectedNode);
                        
                        // Убираем уникальные ноды из доступных
                        if (selectedNode.Tags.Contains("Unique"))
                            allAvailableNodes.Remove(selectedNode);
                    }
                }
            }
            
            currentRankNodes = nextRankNodes;
            if (currentRankNodes.Count == 0) break;
        }
    }

    private void AddNodeConnection(TreeNode parent, TreeNode child)
    {
        // Получаем или создаем запись для родителя
        var parentSaveNode = AddOrGetSaveNode(parent);
        
        // Добавляем ребенка в список детей родителя
        parentSaveNode.nextNodes.Add(child);
        
        // Создаем запись для ребенка (если ее нет)
        AddOrGetSaveNode(child);
    }

    private TreeSaveData.TreeSaveNode AddOrGetSaveNode(TreeNode node)
    {
        var saveNode = gameState.TreeSaveData.allNodes
            .FirstOrDefault(n => n.currentNode == node);
        
        if (saveNode == null)
        {
            saveNode = new TreeSaveData.TreeSaveNode { currentNode = node };
            gameState.TreeSaveData.allNodes.Add(saveNode);
        }
        
        return saveNode;
    }

    

    // Получить все дочерние ноды для конкретной ноды
    public List<TreeNode> GetChildren(TreeNode node)
    {
        if (gameState.TreeSaveData == null) return new List<TreeNode>();
        
        var saveNode = gameState.TreeSaveData.allNodes
            .FirstOrDefault(n => n.currentNode == node);
        return saveNode?.nextNodes ?? new List<TreeNode>();
    }

    // Получить родительские ноды для конкретной ноды
    public List<TreeNode> GetParents(TreeNode node)
    {
        if (gameState.TreeSaveData == null) return new List<TreeNode>();
        
        return gameState.TreeSaveData.allNodes
            .Where(saveNode => saveNode.nextNodes.Contains(node))
            .Select(saveNode => saveNode.currentNode)
            .ToList();
    }

    
    

    public bool CanActivateNode(TreeNode node)
    {
        if (node.IsActive) return false; // Уже активирована
        
        var parents = GetParents(node);
        if (parents.Count == 0) return true; // Корневая нода

        // Проверяем, что все родительские ноды активированы
        return parents.All(parent => parent.IsActive);
    }

    


    private TreeNode SelectNode(TreeNode currentNode, Dictionary<TreeNode, float> possibleNodes, int currentRank)
    {
        if (possibleNodes.Count == 0) return null;
        
        TreeNode node = GetRandomNode(possibleNodes);
        if (node == null) return null;


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
        
        node.Initialize(currentRank);
        return node;
    }

    private Dictionary<TreeNode, float> GetPossibleNodes(TreeNode currentNode, int currentRank)
    {
        var weightedRandomNodes = new Dictionary<TreeNode, float>();
        
        foreach (var node in allAvailableNodes)
        {
            if (node.MinRank <= currentRank && node.MaxRank >= currentRank)
            {
                float weight = CalculateRandomBranchWeight(currentNode, node, allAvailableNodes);
                weightedRandomNodes[node] = weight;
            }
        }
        
        return weightedRandomNodes;
    }

    private float CalculateRandomBranchWeight(TreeNode currentNode, TreeNode node, List<TreeNode> availableNodes)
    {
        float weight = 1;
        
        // Прямое улучшение
        if (node.DirectUpgradeOf == currentNode) 
        { 
            weight += availableNodes.Count / 2; 
        }

        // Совпадение тегов
        foreach (var tag in node.Tags)
        {
            if (currentNode.Tags.Contains(tag)) 
                weight += 2;
        }

        // Бонус за обратную связь в предыдущих нодах
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
    }
}