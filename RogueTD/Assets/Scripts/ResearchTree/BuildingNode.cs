using UnityEngine;

public abstract class BuildingNode : TreeNode
{
    private BuildingBlueprint _buildingBlueprint;
    [SerializeField] protected Building buildingPrefab;
    [SerializeField] protected string buildingName;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int buildingCost;
    [SerializeField] protected Vector2 size;
    
    public BuildingBlueprint BuildingBlueprint
    {
        get
        {
            if (_buildingBlueprint == null)
            {
                _buildingBlueprint = new BuildingBlueprint();
            }
            return _buildingBlueprint;
        }
        set => _buildingBlueprint = value;
    }

    public virtual void CreateBlueprint()
    {
        BuildingBlueprint = new BuildingBlueprint();
        BuildingBlueprint.Initialize(buildingName, buildingPrefab, maxHealthPoints, size );
    }
    public virtual void LoadBasicStats(int rank, float rankMultiplier)
    {
        BuildingBlueprint.Cost = buildingCost;
        BuildingBlueprint.BuildingName = buildingName;
        BuildingBlueprint.BuildingPrefab = buildingPrefab;
        BuildingBlueprint.MaxHealthPoints = maxHealthPoints;
        BuildingBlueprint.Size = size;
    }
    public override int GetDynamicCost(int rank)
    {
        return (int)(Cost * Mathf.Pow(rank, 0.25f));
    }
}