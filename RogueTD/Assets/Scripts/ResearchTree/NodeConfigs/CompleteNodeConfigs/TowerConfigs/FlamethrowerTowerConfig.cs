using UnityEngine;

[CreateAssetMenu(fileName = "FlamethrowerConfig", menuName = "Research Tree/Configs/Turrets/Flamethrower")]
public class FlamethrowerTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Flame Settings")]
    [SerializeField] private float baseDamagePerTick = 10f;
    [SerializeField] private float baseDuration = 4f;
    [SerializeField] private float bonusPerRank;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Flamethrower tower. Burns enemies over time.";

    public float BaseDamagePerTick => baseDamagePerTick;
    public float BaseDuration => baseDuration;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeFlamethrowerTurret(this, rank);
    }

    public float GetRankedDamagePerTick(int rank)
    {
        return  baseDamagePerTick + (baseDamagePerTick * bonusPerRank * rank);
    }
    public float GetRankedDuration(int rank)
    {
        return  baseDuration + (baseDuration * bonusPerRank * rank);
    }
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>{typeof(BurnEffect)};
    }
}