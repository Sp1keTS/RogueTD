using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingConfig", menuName = "Research Tree/Configs/Upgrades/Homing")]
public class HomingConfig : UpgradeTreeNodeConfig
{
    [SerializeField] private float baseRadius = 5f;
    [SerializeField] private float baseRotationSpeed = 5f;
    [SerializeField] private float radiusIncreasePerRank = 0.5f;
    [SerializeField] private float rotationSpeedIncreasePerRank = 1f;
    [SerializeField, TextArea(3, 5)] private string description = "Projectiles track enemies.";

    public float BaseRadius => baseRadius;
    public float BaseRotationSpeed => baseRotationSpeed;
    public float RadiusIncreasePerRank => radiusIncreasePerRank;
    public float RotationSpeedIncreasePerRank => rotationSpeedIncreasePerRank;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new HomingMovementUpgrade(this, rank);
    }
    
    public override List<System.Type> GetConfigResources()
    {
        return new List<System.Type>{typeof(HomingMovement)};
    }
    
    public override bool CheckCompatability(TreeNodeConfig treeNodeConfig)
    {
        return !HasEffectOfType<HomingMovement>(treeNodeConfig, typeof(ProjectileTowerNodeConfig));
    }
}