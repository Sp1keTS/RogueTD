using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    
    string folderPath = "Assets/Resources/BehaviorsAndEffects";
    [SerializeField] Grid constructionGrid;
    [SerializeField] private BuildingBlueprint mainBuildingBlueprint;
    [SerializeField] private ProjectileTowerBlueprint testTowerBlueprint;
    
    void Start()
    {
        instance = this;
        BlueprintManager.blueprints = new Dictionary<string, BuildingBlueprint>(){};
        ConstructionGridManager.constructionGrid = constructionGrid;
        CreateMainBuilding();
        CreateTestTowers();
        CreateAvailableBehaviorsAndEffects();
    }
    
    private void CreateTestTowers()   
    {
        if (testTowerBlueprint == null)
        {
            Debug.LogError("TestTower blueprint is not assigned!");
            return;
        }
        ConstructionManager.projectileTowers = new List<Building>();
        Vector2 gridPosition = MapManager.Size/2 + Vector2.left * 2;
        var tower = BuildingFactory.CreateProjectileTower(gridPosition, testTowerBlueprint);
        Debug.Log(tower);
        ConstructionManager.projectileTowers.Add(tower);
        gridPosition = MapManager.Size/2 + Vector2.right * 4;
        var tower2 = BuildingFactory.CreateProjectileTower(gridPosition, testTowerBlueprint);
        
        ConstructionManager.projectileTowers.Add(tower2);
        BlueprintManager.InsertBlueprint(testTowerBlueprint);
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
    
    private void CreateAvailableBehaviorsAndEffects()
    {
        // Создаем папку если ее не существует
        CreateFolderIfNeeded(folderPath);
        
        ConstructionManager.AvailableProjectileBehaviors = new Dictionary<string, ProjectileBehavior>
        {
            ["homing"] = CreateAsset<HomingMovement>($"{folderPath}/HomingMovement.asset"),
        };

        ConstructionManager.AvailableProjectileEffects = new Dictionary<string, ProjectileEffect>
        {
            ["split"] = CreateAsset<SplitEffect>($"{folderPath}/SplitEffect.asset"),
            ["explosion"] = CreateAsset<ExplosionEffect>($"{folderPath}/ExplosionEffect.asset")
        };

        ConstructionManager.AvailableStatusEffects = new Dictionary<string, StatusEffect>
        {
            ["slow"] = CreateAsset<SlowStatusEffect>($"{folderPath}/SlowStatusEffect.asset"),
            ["bleed"] = CreateAsset<BleedEffect>($"{folderPath}/BleedEffect.asset")
        };

        ConstructionManager.AvailableSecondaryProjectileTowerBehaviors = new Dictionary<string, SecondaryProjectileTowerBehavior>
        {
            ["burst"] = CreateAsset<BurstShotBehavior>($"{folderPath}/BurstShotBehavior.asset"),
            ["cross"] = CreateAsset<CrossShotBehavior>($"{folderPath}/CrossShotBehavior.asset")
        };
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private T CreateAsset<T>(string path) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }
    
    private void CreateFolderIfNeeded(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string[] folders = path.Split('/');
            string currentPath = "";
            
            foreach (string folder in folders)
            {
                if (string.IsNullOrEmpty(currentPath))
                {
                    currentPath = folder;
                }
                else
                {
                    string newPath = currentPath + "/" + folder;
                    if (!AssetDatabase.IsValidFolder(newPath))
                    {
                        AssetDatabase.CreateFolder(currentPath, folder);
                    }
                    currentPath = newPath;
                }
            }
        }
    }
    
    // Статические методы, которые вызывают нестатические через instance
    public static void AddHoming()
    {
        if (instance != null)
            instance.ToggleProjectileBehavior("homing");
    }

    public static void AddSplit()
    {
        if (instance != null)
            instance.ToggleProjectileEffect("split");
    }
    
    public static void AddExplosion()
    {
        if (instance != null)
            instance.ToggleProjectileEffect("explosion");
    }

    public static void AddSlow()
    {
        if (instance != null)
            instance.ToggleStatusEffect("slow");
    }
    
    public static void AddBleed()
    {
        if (instance != null)
            instance.ToggleStatusEffect("bleed");
    }

    public static void AddBurst()
    {
        if (instance != null)
            instance.ToggleSecondaryBehavior("burst");
    }

    public static void AddCross()
    {
        if (instance != null)
            instance.ToggleSecondaryBehavior("cross");
    }

    // Эти методы остаются нестатическими
    private void ToggleProjectileBehavior(string behaviorKey)
    {
        if (testTowerBlueprint == null || !ConstructionManager.AvailableProjectileBehaviors.ContainsKey(behaviorKey))
            return;

        var behavior = ConstructionManager.AvailableProjectileBehaviors[behaviorKey];
        var currentBehaviors = testTowerBlueprint.ProjectileBehaviors?.ToList() ?? new List<ProjectileBehavior>();

        if (currentBehaviors.Contains(behavior))
        {
            currentBehaviors.Remove(behavior);
            Debug.Log($"Removed {behaviorKey} from blueprint");
        }
        else
        {
            currentBehaviors.Add(behavior);
            Debug.Log($"Added {behaviorKey} to blueprint");
        }

        testTowerBlueprint.ProjectileBehaviors = currentBehaviors.ToArray();
        SaveBlueprintChanges();
    }

    private void ToggleProjectileEffect(string effectKey)
    {
        if (testTowerBlueprint == null || !ConstructionManager.AvailableProjectileEffects.ContainsKey(effectKey))
            return;

        var effect = ConstructionManager.AvailableProjectileEffects[effectKey];
        var currentEffects = testTowerBlueprint.ProjectileEffects?.ToList() ?? new List<ProjectileEffect>();

        if (currentEffects.Contains(effect))
        {
            currentEffects.Remove(effect);
            Debug.Log($"Removed {effectKey} from blueprint");
        }
        else
        {
            currentEffects.Add(effect);
            Debug.Log($"Added {effectKey} to blueprint");
        }

        testTowerBlueprint.ProjectileEffects = currentEffects.ToArray();
        SaveBlueprintChanges();
    }

    private void ToggleStatusEffect(string effectKey)
    {
        if (testTowerBlueprint == null || !ConstructionManager.AvailableStatusEffects.ContainsKey(effectKey))
            return;

        var effect = ConstructionManager.AvailableStatusEffects[effectKey];
        var currentEffects = testTowerBlueprint.StatusEffects?.ToList() ?? new List<StatusEffect>();

        if (currentEffects.Contains(effect))
        {
            currentEffects.Remove(effect);
            Debug.Log($"Removed {effectKey} from blueprint");
        }
        else
        {
            currentEffects.Add(effect);
            Debug.Log($"Added {effectKey} to blueprint");
        }

        testTowerBlueprint.StatusEffects = currentEffects.ToArray();
        SaveBlueprintChanges();
    }

    private void ToggleSecondaryBehavior(string behaviorKey)
    {
        if (testTowerBlueprint == null || !ConstructionManager.AvailableSecondaryProjectileTowerBehaviors.ContainsKey(behaviorKey))
            return;

        var behavior = ConstructionManager.AvailableSecondaryProjectileTowerBehaviors[behaviorKey];
        var currentBehaviors = testTowerBlueprint.SecondaryShots?.ToList() ?? new List<SecondaryProjectileTowerBehavior>();

        if (currentBehaviors.Contains(behavior))
        {
            currentBehaviors.Remove(behavior);
            Debug.Log($"Removed {behaviorKey} from blueprint");
        }
        else
        {
            currentBehaviors.Add(behavior);
            Debug.Log($"Added {behaviorKey} to blueprint");
        }

        testTowerBlueprint.SecondaryShots = currentBehaviors.ToArray();
        SaveBlueprintChanges();
    }
    
    private void SaveBlueprintChanges()
    {
        EditorUtility.SetDirty(testTowerBlueprint);
        AssetDatabase.SaveAssets(); 
        BlueprintManager.InsertBlueprint(testTowerBlueprint);
    }
}