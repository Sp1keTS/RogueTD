using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConstructionManager
{


    public static List<Building> projectileTowers;
    public static Dictionary<string, ProjectileBehavior> AvailableProjectileBehaviors;
    public static Dictionary<string, ProjectileEffect> AvailableProjectileEffects;
    public static Dictionary<string, StatusEffect> AvailableStatusEffects;
    public static Dictionary<string, SecondaryProjectileTowerBehavior> AvailableSecondaryProjectileTowerBehaviors;

    void Start()
    {
        

    }
}