using UnityEngine;

public abstract class BuildingNode : TreeNode
{
    private BuildingBlueprint _buildingBlueprint;
    [SerializeField] protected Building buildingPrefab;
    [SerializeField] protected string buildingName;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int buildingCost;
    [SerializeField] protected Vector2 size;
    
    public BuildingBlueprint BuildingBlueprint {get => _buildingBlueprint; set => _buildingBlueprint = value;}

    public virtual void CreateBlueprint()
    {
        _buildingBlueprint = new BuildingBlueprint();
        _buildingBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
    public virtual void LoadBasicStats(int rank, float rankMultiplier)
    {
        _buildingBlueprint.Cost = buildingCost;
        _buildingBlueprint.BuildingName = buildingName;
        _buildingBlueprint.BuildingPrefab = buildingPrefab;
        _buildingBlueprint.MaxHealthPoints = maxHealthPoints;
        _buildingBlueprint.Size = size;
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.25f));
    }
}