using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBlueprint", menuName = "Tower Defense/Building Blueprint")]
public class BuildingBlueprint
{
    protected string _buildingName;
    protected Building _buildingPrefab;
    protected int _maxHealthPoints;
    protected int _cost;
    protected Vector2 _size;
    public Building BuildingPrefab { get => _buildingPrefab; set => _buildingPrefab = value; }
    public int MaxHealthPoints { get => _maxHealthPoints; set => _maxHealthPoints = value; }
    public int Cost { get => _cost; set => _cost = value; }
    public Vector2 Size { get => _size; set => _size = value; }
    public string BuildingName { get => _buildingName; set => _buildingName = value; }

    public void Initialize(string buildingName, Building buildingPrefab, int maxHealthPoints, Vector2 size)
    {
        _buildingName = buildingName;
        _buildingPrefab = buildingPrefab;
        _maxHealthPoints = maxHealthPoints;
        _size = size;
    }
}
