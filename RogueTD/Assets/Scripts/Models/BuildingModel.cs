using System.Numerics;
using JetBrains.Annotations;
using Unity.Mathematics;
using Vector2 = UnityEngine.Vector2;

namespace Models
{
    public class BuildingModel
    {
        protected string buildingName;
        protected int healthPoints;
        protected int maxHealthPoints;
        protected BuildingBlueprint buildingBlueprint;
        protected Vector2 position;
        protected Tower tower;
        
        
        public string BuildingName
        {
            get => buildingName; 
            set => buildingName = value;
        }
        
        public int HealthPoints
        {
            get => healthPoints;
            set => healthPoints = math.clamp(value, 0, maxHealthPoints);
        }

        public int MaxHealthPoints
        {
            get => maxHealthPoints;
            set => maxHealthPoints = value > 0 ? value : 0;
        }
        
        public Vector2 Position { get => position; set => position = value; }
        
        public Vector2 Size => buildingBlueprint.Size;
        
        public Tower Tower { get => tower; set => tower = value; }

        public BuildingModel()
        {
            BuildingName = string.Empty;
        }
        
        public BuildingModel(BuildingBlueprint buildingBlueprint)
        {
            this.buildingName = buildingBlueprint.buildingName;
            this.buildingBlueprint = buildingBlueprint;
            this.maxHealthPoints = buildingBlueprint.MaxHealthPoints;
            healthPoints = maxHealthPoints;
        }

        public void Update(BuildingBlueprint buildingBlueprint)
        {
            this.buildingBlueprint = buildingBlueprint;
            this.maxHealthPoints = buildingBlueprint.MaxHealthPoints;
        }
    }
}