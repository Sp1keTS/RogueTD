using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    [SerializeField] private Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ResearchTree researchTree;
    [SerializeField] private TreeSolver treeSolver;
    

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

        treeSolver.SolveAndDisplayTree();
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