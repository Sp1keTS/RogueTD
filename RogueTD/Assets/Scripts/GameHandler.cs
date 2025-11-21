using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    [SerializeField] private Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ProjectileTowerBlueprint testTowerTemplate;
    
    void Start()
    {
        instance = this;
        ConstructionGridManager.constructionGrid = constructionGrid;
        
        
        ResourceManager.LoadAllResources();
        CreateMainBuilding();
        CreateTestTowers();
    }
    
   
    private void CreateTestTowers()   
    {
        if (testTowerTemplate == null)
        {
            Debug.LogError("TestTower template is not assigned!");
            return;
        }

        BlueprintManager.InsertProjectileTowerBlueprint(testTowerTemplate);
        
        var blueprint = BlueprintManager.GetBlueprint(testTowerTemplate.buildingName) as ProjectileTowerBlueprint;
        if (blueprint == null)
        {
            Debug.LogError("Failed to create blueprint from template");
            return;
        }

        ConstructionManager.projectileTowers = new List<Building>();
        Vector2 gridPosition = MapManager.Size/2 + Vector2.left * 2;
        var tower = BuildingFactory.CreateProjectileTower(gridPosition, blueprint);
        ConstructionManager.projectileTowers.Add(tower);
        
        gridPosition = MapManager.Size/2 + Vector2.right * 4;
        var tower2 = BuildingFactory.CreateProjectileTower(gridPosition, blueprint);
        ConstructionManager.projectileTowers.Add(tower2);
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

    // Статические методы для добавления поведений и эффектов
    public static void AddHoming()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleProjectileBehavior("homing");
    }

    public static void AddSplit()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleProjectileEffect("split");
    }
    
    public static void AddExplosion()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleProjectileEffect("explosion");
    }

    public static void AddSlow()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleStatusEffect("slow");
    }
    
    public static void AddBleed()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleStatusEffect("bleed");
    }

    public static void AddBurst()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleSecondaryBehavior("burst");
    }

    public static void AddCross()
    {
        if (instance != null && instance.testTowerTemplate != null)
            instance.ToggleSecondaryBehavior("cross");
    }

    private void ToggleProjectileBehavior(string behaviorKey)
    {
        var behavior = ResourceManager.GetProjectileBehavior(behaviorKey);
        if (behavior == null)
        {
            Debug.LogError($"Projectile behavior '{behaviorKey}' not found in ResourceManager");
            return;
        }

        var blueprint = BlueprintManager.GetBlueprint(testTowerTemplate.buildingName) as ProjectileTowerBlueprint;
        if (blueprint == null) return;

        List<ResourceReference<ProjectileBehavior>> currentBehaviors = 
            blueprint.ProjectileBehaviors?.ToList() ?? new List<ResourceReference<ProjectileBehavior>>();

        var existingRef = currentBehaviors.FirstOrDefault<ResourceReference<ProjectileBehavior>>(r => r?.Value?.name == behaviorKey);
        
        if (existingRef != null)
        {
            currentBehaviors.Remove(existingRef);
            Debug.Log($"Removed {behaviorKey} from blueprint");
        }
        else
        {
            var newReference = new ResourceReference<ProjectileBehavior> { Value = behavior };
            currentBehaviors.Add(newReference);
            Debug.Log($"Added {behaviorKey} to blueprint");
        }

        blueprint.ProjectileBehaviors = currentBehaviors.ToArray();
        SaveBlueprintChanges(blueprint);
    }

    private void ToggleProjectileEffect(string effectKey)
    {
        var effect = ResourceManager.GetProjectileEffect(effectKey);
        if (effect == null)
        {
            Debug.LogError($"Projectile effect '{effectKey}' not found in ResourceManager");
            return;
        }

        var blueprint = BlueprintManager.GetBlueprint(testTowerTemplate.buildingName) as ProjectileTowerBlueprint;
        if (blueprint == null) return;

        List<ResourceReference<ProjectileEffect>> currentEffects = 
            blueprint.ProjectileEffects?.ToList() ?? new List<ResourceReference<ProjectileEffect>>();

        var existingRef = currentEffects.FirstOrDefault<ResourceReference<ProjectileEffect>>(r => r?.Value?.name == effectKey);
        
        if (existingRef != null)
        {
            currentEffects.Remove(existingRef);
            Debug.Log($"Removed {effectKey} from blueprint");
        }
        else
        {
            var newReference = new ResourceReference<ProjectileEffect> { Value = effect };
            currentEffects.Add(newReference);
            Debug.Log($"Added {effectKey} to blueprint");
        }

        blueprint.ProjectileEffects = currentEffects.ToArray();
        SaveBlueprintChanges(blueprint);
    }

    private void ToggleStatusEffect(string effectKey)
    {
        var effect = ResourceManager.GetStatusEffect(effectKey);
        if (effect == null)
        {
            Debug.LogError($"Status effect '{effectKey}' not found in ResourceManager");
            return;
        }

        var blueprint = BlueprintManager.GetBlueprint(testTowerTemplate.buildingName) as ProjectileTowerBlueprint;
        if (blueprint == null) return;

        List<ResourceReference<StatusEffect>> currentEffects = 
            blueprint.StatusEffects?.ToList() ?? new List<ResourceReference<StatusEffect>>();

        var existingRef = currentEffects.FirstOrDefault<ResourceReference<StatusEffect>>(r => r?.Value?.name == effectKey);
        
        if (existingRef != null)
        {
            currentEffects.Remove(existingRef);
            Debug.Log($"Removed {effectKey} from blueprint");
        }
        else
        {
            var newReference = new ResourceReference<StatusEffect> { Value = effect };
            currentEffects.Add(newReference);
            Debug.Log($"Added {effectKey} to blueprint");
        }

        blueprint.StatusEffects = currentEffects.ToArray();
        SaveBlueprintChanges(blueprint);
    }

    private void ToggleSecondaryBehavior(string behaviorKey)
    {
        var behavior = ResourceManager.GetSecondaryBehavior(behaviorKey);
        if (behavior == null)
        {
            Debug.LogError($"Secondary behavior '{behaviorKey}' not found in ResourceManager");
            return;
        }

        var blueprint = BlueprintManager.GetBlueprint(testTowerTemplate.buildingName) as ProjectileTowerBlueprint;
        if (blueprint == null) return;

        List<ResourceReference<SecondaryProjectileTowerBehavior>> currentBehaviors = 
            blueprint.SecondaryShots?.ToList() ?? new List<ResourceReference<SecondaryProjectileTowerBehavior>>();

        var existingRef = currentBehaviors.FirstOrDefault<ResourceReference<SecondaryProjectileTowerBehavior>>(r => r?.Value?.name == behaviorKey);
        
        if (existingRef != null)
        {
            currentBehaviors.Remove(existingRef);
            Debug.Log($"Removed {behaviorKey} from blueprint");
        }
        else
        {
            var newReference = new ResourceReference<SecondaryProjectileTowerBehavior> { Value = behavior };
            currentBehaviors.Add(newReference);
            Debug.Log($"Added {behaviorKey} to blueprint");
        }

        blueprint.SecondaryShots = currentBehaviors.ToArray();
        SaveBlueprintChanges(blueprint);
    }

    private void SaveBlueprintChanges(ProjectileTowerBlueprint blueprint)
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(blueprint);
        AssetDatabase.SaveAssets();
        #endif
        
        BlueprintManager.InsertProjectileTowerBlueprint(blueprint);
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}