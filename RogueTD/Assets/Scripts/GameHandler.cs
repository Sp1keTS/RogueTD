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
    [SerializeField] private bool geneRateANewTree = false;
    

    void Start()
    {
        instance = this;
        ConstructionGridManager.constructionGrid = constructionGrid;
        
        CreateMainBuilding();
        if (geneRateANewTree){GenerateResearchTree();}
        LoadUITree();
        
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



    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}