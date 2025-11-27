using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeSolver : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private UITreeNode uiTreeNodePrefab;
    [SerializeField] private Transform treeContainer;
    [SerializeField] private float circleRadius = 300f;
    [SerializeField] private float verticalSpacing = 150f;
    [SerializeField] private float horizontalSpacing = 120f;

    private Dictionary<string, UITreeNode> nodeUIElements = new Dictionary<string, UITreeNode>();
    private Dictionary<int, List<ResearchTree.TreeSaveData.TreeSaveNode>> nodesByRank = new Dictionary<int, List<ResearchTree.TreeSaveData.TreeSaveNode>>();
    private Dictionary<string, Vector2> nodePositions = new Dictionary<string, Vector2>();
    
    // Кэш для загруженных TreeNode ассетов
    private Dictionary<string, TreeNode> _nodeAssetCache = new Dictionary<string, TreeNode>();

    public void SolveAndDisplayTree()
    {
        if (gameState?.TreeSaveData == null)
        {
            Debug.LogError("No tree save data found in GameState");
            return;
        }

        Debug.Log($"TreeSolver: {gameState.TreeSaveData.allNodes.Count} nodes in save data, {gameState.TreeSaveData.rootNodeIds.Count} roots");

        ClearTreeUI();
        LoadNodeAssets();
        OrganizeNodesByRank();
        CalculateNodePositions();
        CreateUIElements();
        LoadActiveNodesDependencies();
    }

    private void ClearTreeUI()
    {
        nodeUIElements.Clear();
        nodesByRank.Clear();
        nodePositions.Clear();

        foreach (Transform child in treeContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadNodeAssets()
    {
        _nodeAssetCache.Clear();
        TreeNode[] nodes = Resources.LoadAll<TreeNode>("Nodes");
        foreach (var node in nodes)
        {
            _nodeAssetCache[node.name] = node;
        }
        Debug.Log($"Loaded {_nodeAssetCache.Count} node assets");
    }

    private void OrganizeNodesByRank()
    {
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            int rank = saveNode.rank;

            if (!nodesByRank.ContainsKey(rank))
            {
                nodesByRank[rank] = new List<ResearchTree.TreeSaveData.TreeSaveNode>();
            }
            nodesByRank[rank].Add(saveNode);
        }

        // Сортируем ранги
        var sortedRanks = nodesByRank.Keys.OrderBy(rank => rank).ToList();
        var sortedNodesByRank = new Dictionary<int, List<ResearchTree.TreeSaveData.TreeSaveNode>>();
        foreach (var rank in sortedRanks)
        {
            sortedNodesByRank[rank] = nodesByRank[rank];
        }
        nodesByRank = sortedNodesByRank;

        Debug.Log($"Organized {nodesByRank.Sum(x => x.Value.Count)} nodes into {nodesByRank.Count} ranks");
    }

    private void CalculateNodePositions()
    {
        if (gameState.TreeSaveData.rootNodeIds.Count == 0) return;

        // Позиционируем корневые ноды по кругу
        float angleStep = 360f / gameState.TreeSaveData.rootNodeIds.Count;
        for (int i = 0; i < gameState.TreeSaveData.rootNodeIds.Count; i++)
        {
            string rootNodeId = gameState.TreeSaveData.rootNodeIds[i];
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 position = new Vector2(
                Mathf.Cos(angle) * circleRadius,
                Mathf.Sin(angle) * circleRadius
            );
            nodePositions[rootNodeId] = position;
        }

        // Позиционируем остальные ноды по рангам
        foreach (var rank in nodesByRank.Keys)
        {
            if (rank == 0) continue; // Корни уже размещены

            var rankNodes = nodesByRank[rank];
            float totalWidth = (rankNodes.Count - 1) * horizontalSpacing;
            float startX = -totalWidth / 2f;
            float yPosition = -rank * verticalSpacing;

            for (int i = 0; i < rankNodes.Count; i++)
            {
                var saveNode = rankNodes[i];
                Vector2 position = new Vector2(startX + i * horizontalSpacing, yPosition);
                nodePositions[saveNode.nodeId] = position;
            }
        }
    }

    private void CreateUIElements()
    {
        // Создаем UI элементы для всех нод
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            CreateUITreeNode(saveNode);
        }

        CreateNodeConnections();
    }

    private void CreateUITreeNode(ResearchTree.TreeSaveData.TreeSaveNode saveNode)
    {
        if (uiTreeNodePrefab == null || treeContainer == null) return;

        // Получаем TreeNode ассет
        if (!_nodeAssetCache.TryGetValue(saveNode.nodeAssetName, out var nodeAsset))
        {
            Debug.LogWarning($"Node asset not found: {saveNode.nodeAssetName}");
            return;
        }

        // Создаем UI элемент
        UITreeNode uiNode = Instantiate(uiTreeNodePrefab, treeContainer);
        
        // Восстанавливаем состояние ноды из save data
        nodeAsset.IsActive = saveNode.isActive;
        nodeAsset.CurrentRank = saveNode.rank;
        
        uiNode.SetNode(nodeAsset, saveNode.nodeId);

        if (nodePositions.TryGetValue(saveNode.nodeId, out Vector2 position))
        {
            uiNode.transform.localPosition = position;
        }

        nodeUIElements[saveNode.nodeId] = uiNode;
    }

    private void CreateNodeConnections()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) return;

        List<Vector3> linePositions = new List<Vector3>();

        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            foreach (string childId in saveNode.nextNodeIds)
            {
                if (nodePositions.ContainsKey(saveNode.nodeId) && nodePositions.ContainsKey(childId))
                {
                    linePositions.Add(nodePositions[saveNode.nodeId]);
                    linePositions.Add(nodePositions[childId]);
                }
            }
        }

        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;
    }

    private void LoadActiveNodesDependencies()
    {
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            if (saveNode.isActive && _nodeAssetCache.TryGetValue(saveNode.nodeAssetName, out var nodeAsset))
            {
                nodeAsset.LoadDependencies();
            }
        }
    }

    public bool CanActivateNode(TreeNode node, string nodeId)
    {
        // Корневые ноды всегда доступны для активации
        if (gameState.TreeSaveData.rootNodeIds.Contains(nodeId))
            return true;

        // Для остальных нод проверяем, активированы ли все родители
        var parents = GetParentNodes(nodeId);
        return parents.Count > 0 && parents.All(parent => parent.isActive);
    }

    private List<ResearchTree.TreeSaveData.TreeSaveNode> GetParentNodes(string nodeId)
    {
        var parents = new List<ResearchTree.TreeSaveData.TreeSaveNode>();
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            if (saveNode.nextNodeIds.Contains(nodeId))
            {
                parents.Add(saveNode);
            }
        }
        return parents;
    }

    // Метод для активации ноды
    public void ActivateNode(string nodeId)
    {
        var saveNode = gameState.TreeSaveData.GetNodeById(nodeId);
        if (saveNode != null && !saveNode.isActive)
        {
            saveNode.isActive = true;
            
            // Обновляем состояние TreeNode ассета
            if (_nodeAssetCache.TryGetValue(saveNode.nodeAssetName, out var nodeAsset))
            {
                nodeAsset.IsActive = true;
                nodeAsset.OnActivate();
                nodeAsset.LoadDependencies();
            }
            
            // Обновляем UI
            if (nodeUIElements.TryGetValue(nodeId, out var uiNode))
            {
                uiNode.UpdateVisualState();
            }
        }
    }

    // Получить все дочерние ноды для конкретной ноды
    public List<(TreeNode node, string nodeId)> GetChildren(string nodeId)
    {
        var result = new List<(TreeNode node, string nodeId)>();
        var saveNode = gameState.TreeSaveData.GetNodeById(nodeId);
        
        if (saveNode != null)
        {
            foreach (var childId in saveNode.nextNodeIds)
            {
                var childSaveNode = gameState.TreeSaveData.GetNodeById(childId);
                if (childSaveNode != null && _nodeAssetCache.TryGetValue(childSaveNode.nodeAssetName, out var childNode))
                {
                    // Восстанавливаем состояние ноды
                    childNode.IsActive = childSaveNode.isActive;
                    childNode.CurrentRank = childSaveNode.rank;
                    result.Add((childNode, childId));
                }
            }
        }
        
        return result;
    }

    // Получить TreeNode по nodeId
    public TreeNode GetTreeNodeById(string nodeId)
    {
        var saveNode = gameState.TreeSaveData.GetNodeById(nodeId);
        if (saveNode != null && _nodeAssetCache.TryGetValue(saveNode.nodeAssetName, out var node))
        {
            node.IsActive = saveNode.isActive;
            node.CurrentRank = saveNode.rank;
            return node;
        }
        return null;
    }
}