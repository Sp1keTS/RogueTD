using System.Collections.Generic;
using Models;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    [SerializeField] private Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ProjectileTowerBlueprint testProjectileTowerBlueprint;
    [SerializeField] private ResearchTree researchTree;
    [SerializeField] private TreeSolver treeSolver;
    

    void Start()
    {
        instance = this;
        ConstructionGridManager.constructionGrid = constructionGrid;
        
        CreateMainBuilding();
        CreateTestTowers();
        LoadUITree();
        GenerateResearchTree();
    }

    private void LoadUITree()
    {
        
        
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
        treeSolver.LoadAndSolveTree();
        Debug.Log("Research tree Loaded successfully");
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

    private void CreateTestTowers()
    {
        Vector2 gridPosition = MapManager.Size/2 + Vector2.right * 5; 
        BuildingFactory.CreateProjectileTower(gridPosition, testProjectileTowerBlueprint);
    }



    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}