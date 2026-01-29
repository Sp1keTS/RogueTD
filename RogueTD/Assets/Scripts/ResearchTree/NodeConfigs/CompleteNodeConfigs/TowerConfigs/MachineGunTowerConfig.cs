using UnityEngine;

[CreateAssetMenu(fileName = "MachineGunConfig", menuName = "Research Tree/Configs/Turrets/Machine Gun")]
public class MachineGunTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Magazine Settings")]
    [SerializeField] private int baseMagazineSize = 30;
    [SerializeField] private float baseRampMultiplier = 2f;
    [SerializeField] private float baseRampTime = 3f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Machine gun with ramping fire rate and magazine system.";

    public int BaseMagazineSize => baseMagazineSize;
    public float BaseRampMultiplier => baseRampMultiplier;
    public float BaseRampTime => baseRampTime;
    public string Description => description;

    public override TreeNode CreateNode( int rank)
    {
        return new NodeMachineGunTurret(this, rank);
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>{
            typeof(AmmoBasedShootBehavior), 
            typeof(RampingFireRateBehavior)
        };
    }
}