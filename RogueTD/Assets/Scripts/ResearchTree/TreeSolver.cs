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

    private Dictionary<TreeNode, UITreeNode> nodeUIElements = new Dictionary<TreeNode, UITreeNode>();
    private Dictionary<int, List<TreeNode>> nodesByRank = new Dictionary<int, List<TreeNode>>();
    private Dictionary<TreeNode, Vector2> nodePositions = new Dictionary<TreeNode, Vector2>();

    public void SolveAndDisplayTree()
    {
        if (gameState?.TreeSaveData == null)
        {
            Debug.LogError("No tree save data found in GameState");
            return;
        }

        ClearTreeUI();
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

    private void OrganizeNodesByRank()
    {
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            TreeNode node = saveNode.currentNode;
            int rank = node.CurrentRank;

            if (!nodesByRank.ContainsKey(rank))
            {
                nodesByRank[rank] = new List<TreeNode>();
            }
            nodesByRank[rank].Add(node);
        }

        var sortedRanks = nodesByRank.Keys.OrderBy(rank => rank).ToList();
        var sortedNodesByRank = new Dictionary<int, List<TreeNode>>();
        foreach (var rank in sortedRanks)
        {
            sortedNodesByRank[rank] = nodesByRank[rank];
        }
        nodesByRank = sortedNodesByRank;
    }

    private void CalculateNodePositions()
    {
        if (gameState.TreeSaveData.rootNodes.Count == 0) return;

        float angleStep = 360f / gameState.TreeSaveData.rootNodes.Count;
        for (int i = 0; i < gameState.TreeSaveData.rootNodes.Count; i++)
        {
            TreeNode rootNode = gameState.TreeSaveData.rootNodes[i];
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 position = new Vector2(
                Mathf.Cos(angle) * circleRadius,
                Mathf.Sin(angle) * circleRadius
            );
            nodePositions[rootNode] = position;
        }

        foreach (var rank in nodesByRank.Keys)
        {
            if (rank == 0) continue;

            var rankNodes = nodesByRank[rank];
            float totalWidth = (rankNodes.Count - 1) * horizontalSpacing;
            float startX = -totalWidth / 2f;
            float yPosition = -rank * verticalSpacing;

            for (int i = 0; i < rankNodes.Count; i++)
            {
                TreeNode node = rankNodes[i];
                Vector2 position = new Vector2(startX + i * horizontalSpacing, yPosition);
                nodePositions[node] = position;
            }
        }
    }

    private void CreateUIElements()
    {
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            TreeNode node = saveNode.currentNode;
            CreateUITreeNode(node);
        }

        CreateNodeConnections();
    }

    private void CreateUITreeNode(TreeNode node)
    {
        if (uiTreeNodePrefab == null || treeContainer == null) return;

        UITreeNode uiNode = Instantiate(uiTreeNodePrefab, treeContainer);
        uiNode.SetNode(node);

        if (nodePositions.TryGetValue(node, out Vector2 position))
        {
            uiNode.transform.localPosition = position;
        }

        nodeUIElements[node] = uiNode;
    }

    private void CreateNodeConnections()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) return;

        List<Vector3> linePositions = new List<Vector3>();

        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            TreeNode parent = saveNode.currentNode;
            foreach (TreeNode child in saveNode.nextNodes)
            {
                if (nodePositions.ContainsKey(parent) && nodePositions.ContainsKey(child))
                {
                    linePositions.Add(nodePositions[parent]);
                    linePositions.Add(nodePositions[child]);
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
            if (saveNode.currentNode.IsActive)
            {
                saveNode.currentNode.LoadDependencies();
            }
        }
    }

    public bool CanActivateNode(TreeNode node)
    {
        // Корневые ноды всегда доступны для активации
        if (gameState.TreeSaveData.rootNodes.Contains(node))
            return true;

        // Для остальных нод проверяем, активированы ли все родители
        var parents = GetParentNodes(node);
        return parents.Count > 0 && parents.All(parent => parent.IsActive);
    }

    private List<TreeNode> GetParentNodes(TreeNode node)
    {
        var parents = new List<TreeNode>();
        foreach (var saveNode in gameState.TreeSaveData.allNodes)
        {
            if (saveNode.nextNodes.Contains(node))
            {
                parents.Add(saveNode.currentNode);
            }
        }
        return parents;
    }
}