using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBlueprint", menuName = "Tower Defense/Building Blueprint")]
public class BuildingBlueprint : ScriptableObject
{
    [Header("Base Building Settings")]
    [SerializeField] public string buildingName;
    [SerializeField] protected GameObject buildingPrefab;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int cost;
    [SerializeField] protected Vector2 size;
    public GameObject BuildingPrefab { get => buildingPrefab; set => buildingPrefab = value; }
    public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }
    public int Cost { get => cost; set => cost = value; }
    public Vector2 Size { get => size; set => size = value; }
}
