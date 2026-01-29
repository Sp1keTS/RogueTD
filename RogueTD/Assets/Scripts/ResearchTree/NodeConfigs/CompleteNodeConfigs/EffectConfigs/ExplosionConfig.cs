using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionConfig", menuName = "Research Tree/Configs/Upgrades/Explosion")]
public class ExplosionConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseRadius = 3f;
    [SerializeField] private float baseDamagePercentage = 30f;
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float damageIncreasePerRank = 10f;
    [SerializeField, TextArea(3, 5)] private string description = "Projectiles explode on impact.";

    public float BaseRadius => baseRadius;
    public float BaseDamagePercentage => baseDamagePercentage;
    public float RadiusIncreasePerRank => radiusIncreasePerRank;
    public float DamageIncreasePerRank => damageIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new ExplosionEffectUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(ExplosionEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<ExplosionEffect>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}