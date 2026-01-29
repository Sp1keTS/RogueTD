using UnityEngine;

[CreateAssetMenu(fileName = "RapidFireConfig", menuName = "Research Tree/Configs/Turrets/Rapid Fire")]
public class RapidFireTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Magazine Settings")]
    [SerializeField] private float magazineSizeMultiplier = 1.5f;
    [SerializeField] private float reloadSpeedMultiplier = 1.3f;
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "High fire rate turret with magazine system.";

    public float MagazineSizeMultiplier => magazineSizeMultiplier;
    public float ReloadSpeedMultiplier => reloadSpeedMultiplier;
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeRapidFireTurret(this, rank);
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>{typeof(AmmoBasedShootBehavior)};
    }
}