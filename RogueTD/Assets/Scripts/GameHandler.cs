using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    [SerializeField] private Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ResearchTree researchTree;

    void Start()
    {
        instance = this;
        ConstructionGridManager.constructionGrid = constructionGrid;
        
        CreateMainBuilding();
        GenerateResearchTree();
    }

    private void GenerateResearchTree()
    {
        if (researchTree != null)
        {
            researchTree.GenerateATree();
            Debug.Log("Research tree generated successfully");
        }
        else
        {
            Debug.LogError("ResearchTree is not assigned!");
        }
    }
    
    private void CreateMainBuilding()
    {
        if (mainBuildingBlueprint == null)
        {
            Debug.LogError("MainBuilding blueprint is not assigned!");
            return;
        }
        
        Vector2 gridPosition = MapManager.Size/2;
        Building mainBuilding = BuildingFactory.CreateBuilding(gridPosition, mainBuildingBlueprint);
        
        if (mainBuilding != null)
        {
            Debug.Log("Main building created successfully at position (0,0)");
        }
        else
        {
            Debug.LogError("Failed to create main building");
        }
    }

    // Метод для активации ноды исследования (может вызываться из UI)
    public static void ActivateResearchNode(TreeNode node)
    {
        if (instance != null && instance.researchTree != null)
        {
            // TreeSolver будет обрабатывать активацию через UITreeNode
            Debug.Log($"Attempting to activate research node: {node.name}");
        }
    }

    // Метод для получения доступных для активации нод (для UI)
    public static List<TreeNode> GetAvailableResearchNodes()
    {
        if (instance != null && instance.researchTree != null)
        {
            // TreeSolver будет предоставлять этот список
            return new List<TreeNode>();
        }
        return new List<TreeNode>();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}