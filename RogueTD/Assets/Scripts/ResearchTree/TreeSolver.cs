using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        
        var roots = GameState.Instance.TreeSaveData?.rootSaveNodes;
        
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
        
        var angleStep = (roots.Count > 0) ? 360f / roots.Count : 0f;
        var currentAngle = 0f;
            
        foreach (var root in roots)
        {
            if (root == null) continue;
            
            if (root.currentNode == null && root.currentNodeConfig)
            {
                root.currentNode = root.currentNodeConfig.CreateNode(0);
                root.currentNode.Cost = root.currentNodeConfig.Cost;
                
            }
            
            var direction = AngleToDirection(currentAngle);
            var position = centralPoint + (startRadius * direction);
            
            if (float.IsNaN(position.x) || float.IsNaN(position.y))
            {
                position = centralPoint;
            }
            
            var rootNode = Instantiate(uiTreeNodePrefab, canvas.transform);
            rootNode.transform.localPosition = position;
            rootNode.TreeSaveNode = root;
            rootNode.Rank = 0; 
            rootNode.SetImage();
            
            if (root.IsActive)
            {
                rootNode.Button.interactable = false;
                rootNode.TreeSaveNode.currentNode.OnActivate(0);
            }
            
            nodeToUI[root] = rootNode;
            ProcessNodeBranchBFS(root, position, currentAngle, 1); 
            currentAngle += angleStep;
        }
    }

    private void ProcessNodeBranchBFS(ResearchTree.TreeSaveData.TreeSaveNode startNode, Vector2 startPosition, float startAngle, int startDepth)
    {
        var queue = new Queue<BranchNode>();
        queue.Enqueue(new BranchNode { 
            node = startNode, 
            position = startPosition, 
            angle = startAngle, 
            depth = startDepth 
        });

        var maxDepth = 10;
        var processedCount = 0;
        var maxProcessCount = 200;

        while (queue.Count > 0 && processedCount < maxProcessCount)
        {
            var current = queue.Dequeue();
            
            if (current.node.currentNode == null && current.node.currentNodeConfig)
            {
                current.node.currentNode = current.node.currentNodeConfig.CreateNode(current.depth);
                current.node.currentNode.Cost = current.node.currentNodeConfig.Cost;
            }
            
            if (current.node.currentNode is ProjectileTowerUpgradeTreeNode upgradeNode && 
                current.node.nodeToUpgrade != null)
            {
                SetupUpgradeNode(upgradeNode, current.node);
            }
            
            
            if (current.depth > maxDepth) continue;
            if (current.node.nextSaveNodes == null || current.node.nextSaveNodes.Count == 0) continue;
                
            if (current.node.IsActive && current.node.currentNode != null)
            {
                current.node.currentNode.OnActivate(current.depth);
            }
            
            var childCount = current.node.nextSaveNodes.Count;
            var childAngleStep = (childCount > 1) ? branchAngle / (childCount - 1) : 0f;
            var startChildAngle = current.angle - (branchAngle / 2f);
                
            for (var i = 0; i < childCount; i++)
            {
                var nextSaveNode = current.node.nextSaveNodes[i];
                if (nextSaveNode == null) continue;
                if (nodeToUI.ContainsKey(nextSaveNode)) continue;

                var childAngle = startChildAngle + (childAngleStep * i);
                var childDirection = AngleToDirection(childAngle);
                var childPosition = current.position + (nodeDistance * childDirection);
                
                if (float.IsNaN(childPosition.x) || float.IsNaN(childPosition.y))
                {
                    childPosition = current.position + new Vector2(nodeDistance, 0);
                }

                var uiNode = Instantiate(uiTreeNodePrefab, canvas.transform);
                uiNode.transform.localPosition = childPosition;
                uiNode.TreeSaveNode = nextSaveNode;
                uiNode.Rank = current.depth; 
                
                if (nextSaveNode.currentNode == null && nextSaveNode.currentNodeConfig)
                {
                    nextSaveNode.currentNode = nextSaveNode.currentNodeConfig.CreateNode(current.depth);
                    nextSaveNode.currentNode.Cost = nextSaveNode.currentNodeConfig.Cost;
                }
                
                if (nextSaveNode.currentNode is ProjectileTowerUpgradeTreeNode childUpgradeNode)
                {
                    SetupUpgradeNode(childUpgradeNode, nextSaveNode);
                }
                
                uiNode.SetImage();
                
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
    }
    
    private void SetupUpgradeNode(ProjectileTowerUpgradeTreeNode upgradeNode, ResearchTree.TreeSaveData.TreeSaveNode upgradeSaveNode)
    {
        if (upgradeSaveNode.nodeToUpgrade == null) 
        {
            Debug.LogWarning($"Upgrade node {upgradeSaveNode.currentNodeConfig?.name} has no nodeToUpgrade!");
            return;
        }
        if (upgradeSaveNode.nodeToUpgrade.currentNode is ProjectileTowerNode<ProjectileTowerBlueprint> towerNode)
        {
            upgradeNode.ProjectileTowerBlueprint = towerNode.ProjectileTowerBlueprint;
            Debug.Log($"Linked upgrade {upgradeSaveNode.currentNodeConfig?.name} to tower {upgradeSaveNode.nodeToUpgrade.currentNodeConfig?.name}");
        }
        else if (upgradeSaveNode.nodeToUpgrade.currentNode is ProjectileTowerUpgradeTreeNode parentUpgradeNode)
        {
            upgradeNode.ProjectileTowerBlueprint = parentUpgradeNode.ProjectileTowerBlueprint;
            Debug.Log($"Linked upgrade {upgradeSaveNode.currentNodeConfig?.name} to parent upgrade {upgradeSaveNode.nodeToUpgrade.currentNodeConfig?.name}");
        }
        else
        {
            Debug.LogWarning($"Cannot link upgrade {upgradeSaveNode.currentNodeConfig?.name} - nodeToUpgrade is not a tower or upgrade");
        }
    }
    
    private Vector2 AngleToDirection(float angleDegrees)
    {
        angleDegrees = angleDegrees % 360f;
        if (angleDegrees < 0) angleDegrees += 360f;
        
        var angleRadians = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(
            Mathf.Sin(angleRadians),
            Mathf.Cos(angleRadians)
        );
    }

    
    private struct BranchNode
    {
        public ResearchTree.TreeSaveData.TreeSaveNode node;
        public Vector2 position;
        public float angle;
        public int depth; 
    }
}