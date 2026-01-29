using UnityEngine;

public abstract class BuildingNodeConfig : TreeNodeConfig
{
    [SerializeField] protected GameObject buildingPrefab;
    [SerializeField] protected string buildingName;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int buildingCost;
    [SerializeField] protected Vector2 size;
    
    public Building BuildingPrefab => buildingPrefab.GetComponent<Building>();
    public string BuildingName => buildingName;
    public int MaxHealthPoints => maxHealthPoints;
    public int BuildingCost => buildingCost;
    public Vector2 Size => size;
}
