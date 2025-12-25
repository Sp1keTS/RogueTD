using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BlueprintManager
{
    public static Dictionary<string, BuildingBlueprint> blueprints = new Dictionary<string, BuildingBlueprint>();
    public static event Action<ProjectileTowerBlueprint, string> ProjectileTowerChanged = delegate { };
    public static IReadOnlyDictionary<string, BuildingBlueprint> Blueprints => blueprints;
    
    private static void LoadResources<T>(string folderPath, Dictionary<string, T> dictionary) where T : ScriptableObject
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
    public static void DropBlueprints()
    {
        ClearDictionaries();
        Debug.Log("All runtime resources dropped");
    }
    private static void ClearDictionaries()
    {
        blueprints.Clear();
    }
    
   
    static BlueprintManager()
    {
        if (blueprints == null)
        {
            blueprints = new Dictionary<string, BuildingBlueprint>();
        }
    }

    public static void InsertProjectileTowerBlueprint(ProjectileTowerBlueprint blueprint)
    {
        if (blueprints == null)
        {
            blueprints = new Dictionary<string, BuildingBlueprint>();
        }

        if (blueprint == null || string.IsNullOrEmpty(blueprint.buildingName))
        {
            Debug.LogError("Cannot insert null blueprint or blueprint with empty name");
            return;
        }

        if (blueprints.ContainsKey(blueprint.buildingName))
        {
            blueprints[blueprint.buildingName] = blueprint;
        }
        else
        {
            blueprints.Add(blueprint.buildingName, blueprint);
        }
        
        InvokeBuildingChanged(blueprint, blueprint.buildingName);
    }

    public static void InsertBuildingBlueprint(BuildingBlueprint blueprint)
    {
        if (blueprints == null)
        {
            blueprints = new Dictionary<string, BuildingBlueprint>();
        }

        if (blueprint == null || string.IsNullOrEmpty(blueprint.buildingName))
        {
            Debug.LogError("Cannot insert null blueprint or blueprint with empty name");
            return;
        }

        if (blueprints.ContainsKey(blueprint.buildingName))
        {
            blueprints[blueprint.buildingName] = blueprint;
        }
        else
        {
            blueprints.Add(blueprint.buildingName, blueprint);
        }
        
    }

    private static ResourceReference<T>[] ConvertToResourceReferences<T>(T[] items) where T : ScriptableObject
    {
        if (items == null || items.Length == 0)
            return null;

        var references = new ResourceReference<T>[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            references[i] = new ResourceReference<T> { Value = items[i] };
        }
        return references;
    }

    private static ResourceReference<T> ConvertToResourceReference<T>(T item) where T : ScriptableObject
    {
        if (item == null)
            return null;

        return new ResourceReference<T> { Value = item };
    }

    public static void RemoveBlueprint(string buildingName)
    {
        if (blueprints != null && blueprints.ContainsKey(buildingName))
        {
            blueprints.Remove(buildingName);
        }
    }

    public static BuildingBlueprint GetBlueprint(string buildingName)
    {
        return blueprints != null && blueprints.TryGetValue(buildingName, out var blueprint) ? blueprint : null;
    }

    public static void InvokeBuildingChanged(ProjectileTowerBlueprint blueprint, string buildingName)
    {
        ProjectileTowerChanged?.Invoke(blueprint, buildingName);
    }

    public static void ClearBlueprints()
    {
        blueprints?.Clear();
    }

    public static bool HasBlueprint(string buildingName)
    {
        return blueprints != null && blueprints.ContainsKey(buildingName);
    }

    public static int GetBlueprintCount()
    {
        return blueprints?.Count ?? 0;
    }
}