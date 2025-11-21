// ResourceManager.cs
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ResourceManager
{
    private static readonly Dictionary<string, ProjectileBehavior> _projectileBehaviors = new();
    private static readonly Dictionary<string, ProjectileEffect> _projectileEffects = new();
    private static readonly Dictionary<string, StatusEffect> _statusEffects = new();
    private static readonly Dictionary<string, SecondaryProjectileTowerBehavior> _secondaryBehaviors = new();

    public static IReadOnlyDictionary<string, ProjectileBehavior> ProjectileBehaviors => _projectileBehaviors;
    public static IReadOnlyDictionary<string, ProjectileEffect> ProjectileEffects => _projectileEffects;
    public static IReadOnlyDictionary<string, StatusEffect> StatusEffects => _statusEffects;
    public static IReadOnlyDictionary<string, SecondaryProjectileTowerBehavior> SecondaryBehaviors => _secondaryBehaviors;
    
    private const string BEHAVIORS_FOLDER = "Behaviors";
    private const string EFFECTS_FOLDER = "Effects"; 
    private const string STATUS_FOLDER = "StatusEffects";
    private const string SECONDARY_FOLDER = "SecondaryBehaviors";
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        #if UNITY_EDITOR
        CreateResourceFoldersIfNeeded();
        #endif
        LoadAllResources();
    }

#if UNITY_EDITOR
    private static void CreateResourceFoldersIfNeeded()
    {
        string resourcesPath = "Assets/Resources";
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            Debug.Log("Created Resources folder");
        }

        string[] folders = { BEHAVIORS_FOLDER, EFFECTS_FOLDER, STATUS_FOLDER, SECONDARY_FOLDER };
        
        foreach (string folder in folders)
        {
            string folderPath = $"{resourcesPath}/{folder}";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(resourcesPath, folder);
                Debug.Log($"Created folder: {folderPath}");
            }
        }
        
        AssetDatabase.Refresh();
    }
#endif

    public static void LoadAllResources()
    {
        ClearDictionaries();

        LoadResources<ProjectileBehavior>(BEHAVIORS_FOLDER, _projectileBehaviors);
        LoadResources<ProjectileEffect>(EFFECTS_FOLDER, _projectileEffects);
        LoadResources<StatusEffect>(STATUS_FOLDER, _statusEffects);
        LoadResources<SecondaryProjectileTowerBehavior>(SECONDARY_FOLDER, _secondaryBehaviors);


        Debug.Log($"ResourceManager loaded: {_projectileBehaviors.Count} behaviors, {_projectileEffects.Count} effects");
    }
    public static void DropResources()
    {
        ClearDictionaries();
        
        Debug.Log("All runtime resources dropped");
    }
    
    
    
    private static void ClearDictionaries()
    {
        _projectileBehaviors.Clear();
        _projectileEffects.Clear();
        _statusEffects.Clear();
        _secondaryBehaviors.Clear();
    }
    
    private static void LoadResources<T>(string folderPath, Dictionary<string, T> dictionary) where T : Resource
    {
        T[] resources = Resources.LoadAll<T>(folderPath);
        foreach (T resource in resources)
        {
            if (resource != null && !dictionary.ContainsKey(resource.name))
            {
                dictionary[resource.name] = resource;
            }
        }
    }
    

    public static void RegisterProjectileBehavior(string key, ProjectileBehavior behavior)
    {
        if (behavior != null && !_projectileBehaviors.ContainsKey(key))
        {
            _projectileBehaviors[key] = behavior;
        }
    }
    
    public static T GetResource<T>(string name) where T : ScriptableObject
    {
        var dictionary = GetDictionary<T>();
        return dictionary != null && dictionary.TryGetValue(name, out T resource) ? resource : null;
    }
    private static Dictionary<string, T> GetDictionary<T>() where T : ScriptableObject
    {
        if (typeof(T) == typeof(ProjectileBehavior)) return _projectileBehaviors as Dictionary<string, T>;
        if (typeof(T) == typeof(ProjectileEffect)) return _projectileEffects as Dictionary<string, T>;
        if (typeof(T) == typeof(StatusEffect)) return _statusEffects as Dictionary<string, T>;
        if (typeof(T) == typeof(SecondaryProjectileTowerBehavior)) return _secondaryBehaviors as Dictionary<string, T>;
        return null;
    }
    
    public static void RegisterProjectileEffect(string key, ProjectileEffect effect)
    {
        if (effect != null && !_projectileEffects.ContainsKey(key))
        {
            _projectileEffects[key] = effect;
        }
    }

    public static void RegisterStatusEffect(string key, StatusEffect effect)
    {
        if (effect != null && !_statusEffects.ContainsKey(key))
        {
            _statusEffects[key] = effect;
        }
    }

    public static void RegisterSecondaryBehavior(string key, SecondaryProjectileTowerBehavior behavior)
    {
        if (behavior != null && !_secondaryBehaviors.ContainsKey(key))
        {
            _secondaryBehaviors[key] = behavior;
        }
    }

    public static ProjectileBehavior GetProjectileBehavior(string key)
    {
        return _projectileBehaviors.TryGetValue(key, out var behavior) ? behavior : null;
    }

    public static ProjectileEffect GetProjectileEffect(string key)
    {
        return _projectileEffects.TryGetValue(key, out var effect) ? effect : null;
    }

    public static StatusEffect GetStatusEffect(string key)
    {
        return _statusEffects.TryGetValue(key, out var effect) ? effect : null;
    }

    public static SecondaryProjectileTowerBehavior GetSecondaryBehavior(string key)
    {
        return _secondaryBehaviors.TryGetValue(key, out var behavior) ? behavior : null;
    }
}