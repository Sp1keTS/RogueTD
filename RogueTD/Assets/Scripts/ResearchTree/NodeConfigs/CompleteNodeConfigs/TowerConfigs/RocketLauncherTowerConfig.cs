using UnityEngine;

[CreateAssetMenu(fileName = "RocketLauncherConfig", menuName = "Research Tree/Configs/Turrets/Rocket Launcher")]
public class RocketLauncherTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Rocket Settings")]
    [SerializeField] private float baseExplosionRadius = 4f;
    [SerializeField] private float baseDamagePercentage = 50f;
    [SerializeField] private float projectileSpeedMultiplier = 0.7f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Rocket launcher with explosive area damage.";

    public float BaseExplosionRadius => baseExplosionRadius;
    public float BaseDamagePercentage => baseDamagePercentage;
    public float ProjectileSpeedMultiplier => projectileSpeedMultiplier;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeRocketLauncherTurret(this, rank);
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>{typeof(ExplosionEffect)};
    }
}