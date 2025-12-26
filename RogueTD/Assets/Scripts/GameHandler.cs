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
    [SerializeField] private ConstructionGridManager gridManager;
    

    void Start()
    {
        BlueprintManager.InsertBuildingBlueprint(mainBuildingBlueprint);
        ConstructionGridManager.ConstructionGrid = constructionGrid;
        
        
        if (!GameState.Instance.IsANewRun && GameState.Instance.HasSavedData())
        {
            GameState.Instance.LoadAll();
            
            GameState.Instance.LoadBuildings();
            GameState.Instance.LoadResearchTree();
            LoadUITree();
            uiBlueprintHolder.LoadExistingBlueprints();
            gridManager.RecreateBuildings();
        }
        else
        {
            GameState.Instance.Wave = 1;
            GameState.Instance.IsANewRun = true;
            GameState.Instance.ResetGameState();
            GenerateResearchTree();
            LoadUITree();
            CreateMainBuilding();
            GameState.Instance.Initialize(300, 1);
            GameState.Instance.SaveGameState();
            GameState.Instance.LoadGameState();
        }
        
        instance = this;
        mapManager.CreateMap();
    }

    private void LoadUITree()
    {
        if (treeSolver)
        {
            treeSolver.LoadAndSolveTree();
        }
    }

    private void GenerateResearchTree()
    {
        if (researchTree)
        {
            researchTree.GenerateATree();
        }
    }
    
    private void CreateMainBuilding()
    {
        if (mainBuildingBlueprint == null) return;

        var gridPosition = Vector2Int.zero;
        BlueprintManager.InsertBuildingBlueprint(mainBuildingBlueprint);
        Building mainBuilding = BuildingFactory.CreateBuilding(gridPosition, mainBuildingBlueprint);
        GameState.Instance.SaveBuildings();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}