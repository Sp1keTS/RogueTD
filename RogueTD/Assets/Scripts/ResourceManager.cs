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
    private static readonly Dictionary<string, TreeNode> _treeNodes = new(); 
    public static IReadOnlyDictionary<string, ProjectileBehavior> ProjectileBehaviors => _projectileBehaviors;
    public static IReadOnlyDictionary<string, ProjectileEffect> ProjectileEffects => _projectileEffects;
    public static IReadOnlyDictionary<string, StatusEffect> StatusEffects => _statusEffects;
    public static IReadOnlyDictionary<string, SecondaryProjectileTowerBehavior> SecondaryBehaviors => _secondaryBehaviors;
    public static IReadOnlyDictionary<string, ProjectileTowerBehavior> ProjectileTowerBehaviors => _projectileTowerBehaviors;
    public static IReadOnlyDictionary<string, TreeNode> TreeNodes => _treeNodes;
    
    private const string BEHAVIORS_FOLDER = "Behaviors";
    private const string EFFECTS_FOLDER = "Effects"; 
    private const string STATUS_FOLDER = "StatusEffects";
    private const string SECONDARY_FOLDER = "SecondaryBehaviors";
    private const string PROJECTILE_TOWER_BEHAVIORS_FOLDER = "ProjectileTowerBehaviors";
    private const string TREE_NODES_FOLDER = "Nodes"; 
    
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
        var resourcesPath = "Assets/Resources";
        if (!AssetDatabase.IsValidFolder(resourcesPath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
            Debug.Log("Created Resources folder");
        }

        string[] folders = { 
            BEHAVIORS_FOLDER, 
            EFFECTS_FOLDER, 
            STATUS_FOLDER, 
            SECONDARY_FOLDER, 
            PROJECTILE_TOWER_BEHAVIORS_FOLDER,
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

    public static void LoadAllResources()
    {
        ClearDictionaries();

        LoadResources<ProjectileBehavior>(BEHAVIORS_FOLDER, _projectileBehaviors);
        LoadResources<ProjectileEffect>(EFFECTS_FOLDER, _projectileEffects);
        LoadResources<StatusEffect>(STATUS_FOLDER, _statusEffects);
        LoadResources<SecondaryProjectileTowerBehavior>(SECONDARY_FOLDER, _secondaryBehaviors);
        LoadResources<ProjectileTowerBehavior>(PROJECTILE_TOWER_BEHAVIORS_FOLDER, _projectileTowerBehaviors);
        LoadResources<TreeNode>(TREE_NODES_FOLDER, _treeNodes); 

        Debug.Log($"ResourceManager loaded: {_projectileBehaviors.Count} behaviors, " +
                 $"{_projectileEffects.Count} effects, {_projectileTowerBehaviors.Count} tower behaviors, " +
                 $"{_treeNodes.Count} tree nodes"); 
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
        _projectileTowerBehaviors.Clear();
        _treeNodes.Clear(); // ← ДОБАВИЛИ
    }
    
    private static void LoadResources<T>(string folderPath, Dictionary<string, T> dictionary) where T : ScriptableObject
    {
        var resources = Resources.LoadAll<T>(folderPath);
        foreach (var resource in resources)
        {
            if (resource && !dictionary.ContainsKey(resource.name))
            {
                dictionary[resource.name] = resource;
            }
        }
    }


    public static void RegisterTreeNode(string key, TreeNode node)
    {
        if (node && !_treeNodes.ContainsKey(key))
        {
            _treeNodes[key] = node;
            Debug.Log($"TreeNode registered: {key}");
        }
    }

    public static TreeNode GetTreeNode(string key)
    {
        if (_treeNodes.TryGetValue(key, out var node))
        {
            return node;
        }
        
        var loadedNode = Resources.Load<TreeNode>($"{TREE_NODES_FOLDER}/{key}");
        if (loadedNode)
        {
            _treeNodes[key] = loadedNode;
            return loadedNode;
        }
        
        return null;
    }

    public static T GetTreeNode<T>(string key) where T : TreeNode
    {
        var node = GetTreeNode(key);
        return node as T;
    }

    public static List<TreeNode> GetAllTreeNodes()
    {
        return new List<TreeNode>(_treeNodes.Values);
    }

    public static List<TreeNode> GetTreeNodesByTag(string tag)
    {
        var result = new List<TreeNode>();
        foreach (var node in _treeNodes.Values)
        {
            if (node.Tags != null && System.Array.IndexOf(node.Tags, tag) >= 0)
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

    public static ProjectileTowerBehavior GetTowerBehavior(string key)
    {
        return _projectileTowerBehaviors.TryGetValue(key, out var behavior) ? behavior : null;
    }

    public static void RegisterProjectileBehavior(string key, ProjectileBehavior behavior)
    {
        if (behavior && !_projectileBehaviors.ContainsKey(key))
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
        if (typeof(T) == typeof(ProjectileTowerBehavior)) return _projectileTowerBehaviors as Dictionary<string, T>;
        if (typeof(T) == typeof(TreeNode)) return _treeNodes as Dictionary<string, T>; // ← ДОБАВИЛИ
        return null;
    }
    
    public static void RegisterProjectileEffect(string key, ProjectileEffect effect)
    {
        if (effect && !_projectileEffects.ContainsKey(key))
        {
            _projectileEffects[key] = effect;
        }
    }

    public static void RegisterStatusEffect(string key, StatusEffect effect)
    {
        if (effect && !_statusEffects.ContainsKey(key))
        {
            _statusEffects[key] = effect;
        }
    }

    public static void RegisterSecondaryBehavior(string key, SecondaryProjectileTowerBehavior behavior)
    {
        if (behavior && !_secondaryBehaviors.ContainsKey(key))
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