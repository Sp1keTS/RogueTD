using UnityEngine;

[CreateAssetMenu(fileName = "BasicTurretConfig", menuName = "Research Tree/Configs/Turrets/Basic")]
public class BasicTurretConfig : ProjectileTowerNodeConfig
{
    [Header("Description")]
    [SerializeField, TextArea(3, 5)] private string description = 
        "Balanced all-rounder tower with no special abilities.";

    public string Description => description;

    public override TreeNode CreateNode( int rank)
    {
        return new NodeBasicTurret(this,  rank);
    }
    
    public override System.Collections.Generic.List<System.Type> GetConfigResources()
    {
        return new System.Collections.Generic.List<System.Type>();
    }
}