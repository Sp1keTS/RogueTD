using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileTowerBlueprint", menuName = "Tower Defense/ProjectileTowerBlueprint")]
public class ProjectileTowerBlueprint : TowerBlueprint
{
    [SerializeField] protected GameObject buildingPrefab;
    [SerializeField] protected GameObject towerPrefab;
    [SerializeField] protected int damage;

}
