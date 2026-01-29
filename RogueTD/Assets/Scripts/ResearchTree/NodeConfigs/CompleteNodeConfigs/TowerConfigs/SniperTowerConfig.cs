using UnityEngine;

[CreateAssetMenu(fileName = "SniperConfig", menuName = "Research Tree/Configs/Turrets/Sniper")]
public class SniperTowerConfig : ProjectileTowerNodeConfig
{
    [Header("Sniper Settings")]
    
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Long-range turret with high damage but slow fire rate.";
    public string Description => description;

    public override TreeNode CreateNode(int rank)
    {
        return new NodeSniperTurret(this, rank);
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>();
    }
}