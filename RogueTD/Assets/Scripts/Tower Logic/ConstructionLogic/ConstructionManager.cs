using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ConstructionManager
{
    //хранит все доступные строения, а так же не чертёжные ресурсы 
    public static List<Building> projectileTowers;
    public static Dictionary<string, ProjectileBehavior> AvailableProjectileBehaviors;
    public static Dictionary<string, ProjectileEffect> AvailableProjectileEffects;
    public static Dictionary<string, StatusEffect> AvailableStatusEffects;
    public static Dictionary<string, SecondaryProjectileTowerBehavior> AvailableSecondaryProjectileTowerBehaviors;

    static ConstructionManager()
    {
        projectileTowers = new List<Building>();
        AvailableProjectileBehaviors = new Dictionary<string, ProjectileBehavior>();
        AvailableProjectileEffects = new Dictionary<string, ProjectileEffect>();
        AvailableStatusEffects = new Dictionary<string, StatusEffect>();
        AvailableSecondaryProjectileTowerBehaviors = new Dictionary<string, SecondaryProjectileTowerBehavior>();
    }
}