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
    private static readonly Dictionary<string, ProjectileTowerBehavior> _projectileTowerBehaviors = new();
    private static readonly Dictionary<string, TreeNodeConfig>  _treeNodeConfigs = new(); 
    public static IReadOnlyDictionary<string, ProjectileBehavior> ProjectileBehaviors => _projectileBehaviors;
    public static IReadOnlyDictionary<string, ProjectileEffect> ProjectileEffects => _projectileEffects;
    public static IReadOnlyDictionary<string, StatusEffect> StatusEffects => _statusEffects;
    public static IReadOnlyDictionary<string, SecondaryProjectileTowerBehavior> SecondaryBehaviors => _secondaryBehaviors;
    public static IReadOnlyDictionary<string, ProjectileTowerBehavior> ProjectileTowerBehaviors => _projectileTowerBehaviors;
    public static IReadOnlyDictionary<string, TreeNodeConfig> TreeNodeConfigConfigs => _treeNodeConfigs;
    
    private const string TREE_NODES_FOLDER = "Nodes"; 
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        #if UNITY_EDITOR
        CreateResourceFoldersIfNeeded();
        #endif
    }

#if UNITY_EDITOR
    private static void CreateResourceFoldersIfNeeded()
    {
        var resourcesPath = "Assets/Resources";
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            Debug.Log("Created Resources folder");
        }

        string[] folders = { 
            TREE_NODES_FOLDER 
        };
        
        foreach (string folder in folders)
        {
            var folderPath = $"{resourcesPath}/{folder}";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(resourcesPath, folder);
                Debug.Log($"Created folder: {folderPath}");
            }
        }
        
        AssetDatabase.Refresh();
    }
#endif
    
    private static void ClearDictionaries()
    {
        _projectileBehaviors.Clear();
        _projectileEffects.Clear();
        _statusEffects.Clear();
        _secondaryBehaviors.Clear();
        _projectileTowerBehaviors.Clear();
        _treeNodeConfigs.Clear(); 
    }
    
    


    public static TreeNodeConfig GetTreeNodeConfig(string key)
    {
        if (_treeNodeConfigs.TryGetValue(key, out var node))
        {
            return node;
        }
        
        var loadedNode = Resources.Load<TreeNodeConfig>($"{TREE_NODES_FOLDER}/{key}");
        if (loadedNode)
        {
            _treeNodeConfigs[key] = loadedNode;
            return loadedNode;
        }
        
        return null;
    }

    public static T GetTreeNodeConfig<T>(string key) where T : TreeNode
    {
        var node = GetTreeNodeConfig(key);
        return node as T;
    }

    public static List<TreeNodeConfig> GetAllTreeNodes()
    {
        return new List<TreeNodeConfig>(_treeNodeConfigs.Values);
    }

    public static List<TreeNodeConfig> GetTreeNodesByTag(string tag)
    {
        var result = new List<TreeNodeConfig>();
        foreach (var node in _treeNodeConfigs.Values)
        {
            if (node.UtillityTags != null && System.Array.IndexOf(node.UtillityTags, tag) >= 0)
            {
                result.Add(node);
            }
        }
        return result;
    }


    public static void RegisterTowerBehavior(string key, ProjectileTowerBehavior behavior)
    {
        if (behavior != null && !_projectileTowerBehaviors.ContainsKey(key))
        {
            _projectileTowerBehaviors[key] = behavior;
        }
    }


    public static void RegisterProjectileBehavior(string key, ProjectileBehavior behavior)
    {
        if (behavior != null && !_projectileBehaviors.ContainsKey(key))
        {
            _projectileBehaviors[key] = behavior;
        }
    }
    
    public static T GetResource<T>(string name) where T : Resource
    {
        var dictionary = GetDictionary<T>();
        return dictionary != null && dictionary.TryGetValue(name, out T resource) ? resource : null;
    }

    private static Dictionary<string, T> GetDictionary<T>() where T : Resource
    {
        if (typeof(T) == typeof(ProjectileBehavior)) return _projectileBehaviors as Dictionary<string, T>;
        if (typeof(T) == typeof(ProjectileEffect)) return _projectileEffects as Dictionary<string, T>;
        if (typeof(T) == typeof(StatusEffect)) return _statusEffects as Dictionary<string, T>;
        if (typeof(T) == typeof(SecondaryProjectileTowerBehavior)) return _secondaryBehaviors as Dictionary<string, T>;
        if (typeof(T) == typeof(ProjectileTowerBehavior)) return _projectileTowerBehaviors as Dictionary<string, T>;
        if (typeof(T) == typeof(TreeNode)) return _treeNodeConfigs as Dictionary<string, T>; 
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
}