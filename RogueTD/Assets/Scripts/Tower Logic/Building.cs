using Models;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingModel buildingModel;
    [SerializeField] BuildingBlueprint buildingBlueprint;

    void Awake()
    {
        this.buildingModel = new BuildingModel();
    }

    public void TakeDamage(int damage)
    {
        buildingModel.HealthPoints -= damage;
        if (buildingModel.HealthPoints <= 0)
        {
            BuildingFactory.DestroyBuilding(this);
        }
    }

    public virtual void InitializeFromBlueprint(BuildingBlueprint blueprint)
    {
        this.buildingModel = new BuildingModel(blueprint);
    }

    public string BuildingName {get => buildingModel.BuildingName; set => buildingModel.BuildingName =value; }//пока костыль
    public int HealthPoints => buildingModel.HealthPoints;
    public int MaxHealthPoints => buildingModel.MaxHealthPoints;
    public Vector2 Position
    {
        get => buildingModel.Position;
        set => buildingModel.Position = value;
    }
    public Tower Tower { get => buildingModel.Tower; set => buildingModel.Tower = value; }
}
