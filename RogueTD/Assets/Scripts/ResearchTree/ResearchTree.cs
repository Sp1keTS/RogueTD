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
            public int number;
            public bool isActive;
            public List<string> nextNodeIds = new List<string>(); // Следующие ноды
        }

        public List<TreeSaveNode> allNodes = new List<TreeSaveNode>();
        public List<string> rootNodeIds = new List<string>();
    }

    private List<TreeNode> roots = new List<TreeNode>();
    [SerializeField] private GameState gameState;
    private List<TreeNode> allAvailableNodes = new List<TreeNode>();
    private Dictionary<TreeNode, float> _weightedNodes = new Dictionary<TreeNode, float>();
    
    private int _maxRank = 9;
    [SerializeField] private int rootCount = 4;

    // Словарь для хранения связей между нодами (родитель -> дети)
    private Dictionary<TreeNode, List<TreeNode>> nodeChildren = new Dictionary<TreeNode, List<TreeNode>>();

    public void GenerateATree()
    {
        // Инициализируем сохранение и временные структуры
        gameState.TreeSaveData = new TreeSaveData();
        
        LoadAllNodes();
        ClearDependencies();
        CreateRoots();
        CreateBranches();
        SaveFinalTreeStructure();
        UnloadAllNodes();
    }

    private void LoadAllNodes()
    {
        TreeNode[] nodes = Resources.LoadAll<TreeNode>("Nodes");
        allAvailableNodes.AddRange(nodes);
        
    }

    private void ClearDependencies()
    {
        nodeChildren.Clear();
        foreach (TreeNode node in allAvailableNodes)
        {
            node.Number = -1;
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
                nodeChildren[root] = new List<TreeNode>(); // Инициализируем список детей
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
                var children = new List<TreeNode>();
                
                for (int i = 0; i < branchCount; i++)
                {
                    var possibleNodes = GetPossibleNodes(currentNode, currentRank);
                    var selectedNode = SelectNode(currentNode, possibleNodes, currentRank);
                    
                    if (selectedNode != null)
                    {
                        // Создаем связи с предыдущими нодами (для внутренней логики ноды)
                        var previousNodesList = new List<TreeNode>();
                        if (currentNode.PreviousNodes != null)
                            previousNodesList.AddRange(currentNode.PreviousNodes);
                        previousNodesList.Add(currentNode);
                        selectedNode.PreviousNodes = previousNodesList.ToArray();
                        
                        // Добавляем в детей текущей ноды
                        children.Add(selectedNode);
                        nextRankNodes.Add(selectedNode);
                        
                        // Инициализируем список детей для новой ноды
                        nodeChildren[selectedNode] = new List<TreeNode>();
                        
                        // Убираем уникальные ноды из доступных
                        if (selectedNode.Tags.Contains("Unique"))
                            allAvailableNodes.Remove(selectedNode);
                    }
                }
                
                // Сохраняем детей для текущей ноды
                nodeChildren[currentNode] = children;
            }
            
            currentRankNodes = nextRankNodes;
            if (currentRankNodes.Count == 0) break;
        }
    }

    private void SaveFinalTreeStructure()
    {
        // Сохраняем все ноды с их детьми
        foreach (var kvp in nodeChildren)
        {
            var node = kvp.Key;
            var children = kvp.Value;
            
            var saveNode = new TreeSaveData.TreeSaveNode
            {
                nodeId = node.name,
                number = node.Number,
                isActive = node.IsActive
            };

            // Сохраняем ссылки на детей
            foreach (var child in children)
            {
                saveNode.nextNodeIds.Add(child.name);
            }

            gameState.TreeSaveData.allNodes.Add(saveNode);
        }

        // Сохраняем корневые ноды
        foreach (var root in roots)
        {
            gameState.TreeSaveData.rootNodeIds.Add(root.name);
        }
    }

    private TreeNode SelectNode(TreeNode currentNode, Dictionary<TreeNode, float> possibleNodes, int currentRank)
    {
        if (possibleNodes.Count == 0) return null;
        
        TreeNode node = GetRandomNode(possibleNodes);
        if (node == null) return null;

        // Обработка UpgradeTreeNode
        if (node is UpgradeTreeNode upgradeNode)
        {
            var weightedTowerNodes = new Dictionary<TreeNode, float>();
            
            if (currentNode.PreviousNodes != null)
            {
                foreach (var prevNode in currentNode.PreviousNodes)
                {
                    if (prevNode.Tags.Contains("Tower"))
                    {
                        weightedTowerNodes[prevNode] = 1;
                    }
                }
            }
            
            
            if (weightedTowerNodes.Count > 0)
            {
                upgradeNode.ToUpgrade = GetRandomNode(weightedTowerNodes);
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
        
        //  прямое улучшение
        if (node.DirectUpgradeOf == currentNode) 
        { 
            weight += availableNodes.Count / 2; 
        }

        // совпадение тегов
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
        nodeChildren.Clear();
    }

    
}