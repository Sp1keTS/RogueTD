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
        ConstructionGridManager.ConstructionGrid = constructionGrid;
        
        if (GameState.Instance.IsANewRun)
        {
            GameState.Instance.ResetGameState();
            GenerateResearchTree();
            LoadUITree();
            GameState.Instance.Initialize(300, 1);
            GameState.Instance.Wave = 1;
        }
        else
        {
            GameState.Instance.LoadBuildings();
            GameState.Instance.LoadResearchTree();
            LoadUITree();
            uiBlueprintHolder.LoadExistingBlueprints();
            gridManager.RecreateBuildings();
        }
        
        
        instance = this;
        
        mapManager.CreateMap();
        CreateMainBuilding();
    }

    private void LoadUITree()
    {
        if (treeSolver != null)
        {
            treeSolver.LoadAndSolveTree();
        }
    }

    private void GenerateResearchTree()
    {
        if (researchTree != null)
        {
            researchTree.GenerateATree();
        }
    }
    
    private void CreateMainBuilding()
    {
        if (mainBuildingBlueprint == null) return;

        var gridPosition = Vector2Int.zero;
        Building mainBuilding = BuildingFactory.CreateBuilding(gridPosition, mainBuildingBlueprint);
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}