using UnityEngine;
using System.Collections.Generic;

public class TreeSolver : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] private UITreeNode uiTreeNodePrefab;
    [SerializeField] private int startRadius = 200;
    [SerializeField] private float nodeDistance = 100f;
    [SerializeField] private float branchAngle = 45f;
    private Dictionary<ResearchTree.TreeSaveData.TreeSaveNode, UITreeNode> nodeToUI = 
        new Dictionary<ResearchTree.TreeSaveData.TreeSaveNode, UITreeNode>();

    
    public void LoadAndSolveTree()
    {
        nodeToUI.Clear();
        
        var roots = GameState.Instance.TreeSaveData.rootSaveNodes;
        
        if (roots == null || roots.Count == 0)
        {
            Debug.LogError("No root nodes found!");
            return;
        }

        var canvasRect = canvas.GetComponent<RectTransform>();
        if (canvasRect == null)
        {
            Debug.LogError("Canvas RectTransform not found!");
            return;
        }

        var centralPoint = canvasRect.rect.center;
        
        float angleStep = (roots.Count > 0) ? 360f / roots.Count : 0f;
        float currentAngle = 0f;
            
        foreach (var root in roots)
        {
            if (root == null) continue;
            ProcessBuildingNode(root);
            var direction = AngleToDirection(currentAngle);
            var position = centralPoint + (startRadius * direction);
            
            if (float.IsNaN(position.x) || float.IsNaN(position.y))
            {
                Debug.LogError($"Invalid position calculated for root node: {position}");
                position = centralPoint;
            }
            
            var rootNode = Instantiate(uiTreeNodePrefab, canvas.transform);
            rootNode.transform.localPosition = position;
            rootNode.TreeSaveNode = root;
            rootNode.SetImage();
            if (rootNode.TreeSaveNode.IsActive)
            {
                rootNode.Button.interactable = false;
            }
            nodeToUI[root] = rootNode;

            ProcessNodeBranchBFS(root, position, currentAngle);

            currentAngle += angleStep;
        }
    }

    private void ProcessNodeBranchBFS(ResearchTree.TreeSaveData.TreeSaveNode startNode, Vector2 startPosition, float startAngle)
    {
        var queue = new Queue<BranchNode>();
        queue.Enqueue(new BranchNode { node = startNode, position = startPosition, angle = startAngle, depth = 0 });

        var maxDepth = 10;
        var processedCount = 0;
        var maxProcessCount = 100;

        while (queue.Count > 0 && processedCount < maxProcessCount)
        {
            var current = queue.Dequeue();
            
            ProcessBuildingNode(current.node);
            if (current.depth > maxDepth)
            {
                Debug.LogWarning("Max depth reached!");
                continue;
            }

            if (current.node.nextSaveNodes == null || current.node.nextSaveNodes.Count == 0)
                continue;
            if (current.node.IsActive && current.node.currentNode)
            {
                Debug.Log(current.node.currentNode + " " +  current.node.IsActive);
                current.node.currentNode.LoadDependencies();
            }

            
            var childCount = current.node.nextSaveNodes.Count;
            var childAngleStep = (childCount > 1) ? branchAngle / (childCount - 1) : 0f;
            var startChildAngle = current.angle - (branchAngle / 2f);
                
            for (var i = 0; i < childCount; i++)
            {
                var nextSaveNode = current.node.nextSaveNodes[i];
                if (nextSaveNode == null) continue;

                if (nodeToUI.ContainsKey(nextSaveNode))
                    continue;

                var childAngle = startChildAngle + (childAngleStep * i);
                var childDirection = AngleToDirection(childAngle);
                var childPosition = current.position + (nodeDistance * childDirection);
                
                if (float.IsNaN(childPosition.x) || float.IsNaN(childPosition.y))
                {
                    Debug.LogError($"Invalid child position calculated: {childPosition}");
                    childPosition = current.position + new Vector2(nodeDistance, 0);
                }

                var uiNode = Instantiate(uiTreeNodePrefab, canvas.transform);
                uiNode.transform.localPosition = childPosition;
                uiNode.TreeSaveNode = nextSaveNode;
                uiNode.SetImage();
                if (nextSaveNode.nodeToUpgrade != null)
                {
                    uiNode.towerToUpgrade = nextSaveNode.nodeToUpgrade;
                }
                nodeToUI[nextSaveNode] = uiNode;
                if (nextSaveNode.IsActive)
                {
                    uiNode.Button.interactable = false;
                }
                queue.Enqueue(new BranchNode 
                { 
                    node = nextSaveNode, 
                    position = childPosition, 
                    angle = childAngle,
                    depth = current.depth + 1 
                });

                processedCount++;
            }
        }

        if (processedCount >= maxProcessCount)
        {
            Debug.LogWarning("Max process count reached in branch processing");
        }
    }
    
    private Vector2 AngleToDirection(float angleDegrees)
    {
        angleDegrees = angleDegrees % 360f;
        if (angleDegrees < 0) angleDegrees += 360f;
        
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(
            Mathf.Sin(angleRadians),
            Mathf.Cos(angleRadians)
        );
    }

    private void ProcessBuildingNode(ResearchTree.TreeSaveData.TreeSaveNode node)
    {
        if (node == null) return;
    
        if (node.currentNode is ProjectileTowerNode towerNode)
        {
            if (node.IsActive)
            {
                if (!BlueprintManager.HasBlueprint(towerNode.TowerBlueprint.buildingName))
                {
                    BlueprintManager.InsertProjectileTowerBlueprint(towerNode.TowerBlueprint);
                }
            }
        }
    }
    private struct BranchNode
    {
        public ResearchTree.TreeSaveData.TreeSaveNode node;
        public Vector2 position;
        public float angle;
        public int depth;
    }
}