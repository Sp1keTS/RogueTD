using System.Collections.Generic;
using Models;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    [SerializeField] private Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ResearchTree researchTree;
    [SerializeField] private TreeSolver treeSolver;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private UIBlueprintHolderScript uiBlueprintHolder;
    [SerializeField] private GameState gameState;
    [SerializeField] private ConstructionGridManager gridManager;
    

    void Start()
    {
        
        ConstructionGridManager.ConstructionGrid = constructionGrid;
        
        if (gameState.IsANewRun)
        {
            GenerateResearchTree();
            LoadUITree();
            gameState.ResetGameState();
            gameState.IsANewRun = false;
            gameState.Initialize(300, 1);
        }
        else
        {
            LoadUITree();
            gridManager.RecreateBuildings();
            uiBlueprintHolder.LoadExistingBlueprints();
        }
        instance = this;
        
        mapManager.CreateMap();
        CreateMainBuilding();
        
        
        
    }

    private void LoadUITree()
    {
        treeSolver.LoadAndSolveTree();
        Debug.Log("Research tree Loaded successfully");
        
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

        var gridPosition = Vector2Int.zero;
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



    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}