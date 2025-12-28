using UnityEngine;

public static class EffectUtils
{
    public static void AddEffectToBlueprint<T>(ProjectileTowerBlueprint blueprint, T effect, System.Func<ProjectileTowerBlueprint, ResourceReference<T>[]> getArray, System.Action<ProjectileTowerBlueprint, ResourceReference<T>[]> setArray) 
        where T : ScriptableObject
    {
        var currentArray = getArray(blueprint);
        
        if (currentArray == null)
        {
            setArray(blueprint, new ResourceReference<T>[] 
            { 
                new ResourceReference<T> { Value = effect } 
            });
        }
        else
        {
            var effectExists = false;
            foreach (var existingEffect in currentArray)
            {
                if (existingEffect != null && existingEffect.Value != null && 
                    existingEffect.Value.name == effect.name)
                {
                    effectExists = true;
                    break;
                }
            }
            
            if (!effectExists)
            {
                var newEffects = new ResourceReference<T>[currentArray.Length + 1];
                currentArray.CopyTo(newEffects, 0);
                newEffects[^1] = new ResourceReference<T> { Value = effect };
                setArray(blueprint, newEffects);
            }
        }
    }
}