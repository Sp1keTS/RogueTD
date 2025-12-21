using System;
using UnityEngine;

[Serializable]
public class ResourceReference<T> where T : ScriptableObject
{
    [SerializeField] private string resourceName;
    [SerializeField] private T directReference; 
    
    public T Value
    {
        get
        {
            if (!string.IsNullOrEmpty(resourceName))
            {
                var fromManager = ResourceManager.GetResource<T>(resourceName);
                if (fromManager != null) return fromManager;
            }
            
            return directReference;
        }
        set
        {
            directReference = value;
            resourceName = value?.name ?? string.Empty;
        }
    }
    
    public static implicit operator T(ResourceReference<T> reference) 
        => reference?.Value;
        
    public static implicit operator ResourceReference<T>(T resource) 
        => new ResourceReference<T> { Value = resource };
        
    public bool IsValid => !string.IsNullOrEmpty(resourceName) || directReference != null;
    public string ResourceName => resourceName;
}