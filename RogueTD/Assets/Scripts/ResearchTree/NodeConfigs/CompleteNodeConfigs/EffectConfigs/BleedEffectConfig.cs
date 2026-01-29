using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BleedEffectConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float damageIncreasePerRank = 5f;
    [SerializeField] private float durationIncreasePerRank = 0.5f;
    [SerializeField] private float baseDuration;

    public float DamageIncreasePerRank => damageIncreasePerRank;
    public float DurationIncreasePerRank => durationIncreasePerRank;
    public float BaseDuration => baseDuration;

    public override TreeNode CreateNode(int rank)
    {
        return new BleedEffectUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(BleedEffect)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<BleedEffect>(treeNodeConfig, typeof(TowerNodeConfig), typeof(ProjectileTowerNodeConfig)) ;
    }
}
