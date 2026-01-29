using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class TreeNodeConfig : ScriptableObject
{
    [SerializeField] protected Enums.UtillityTags[] utillityTags;
    [SerializeField] protected Enums.GroupTags[] groupTags;
    [SerializeField] protected Enums.GroupTags[] incompatabillityTags;
    [SerializeField] protected int maxRank = 9;
    [SerializeField] protected int minRank = 0;
    [SerializeField] protected TreeNodeConfig directUpgradeOf;
    [SerializeField] protected int cost;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string tooltipText = "Description failed to load properly";
    abstract public List<Type> GetConfigResources();
    public abstract TreeNode CreateNode(int rank);
    
    
    public virtual string TooltipText => tooltipText;
    public int MaxRank => maxRank;
    public int MinRank => minRank;
    public Sprite Icon => icon;
    public TreeNodeConfig DirectUpgradeOf => directUpgradeOf;
    public Enums.UtillityTags[] UtillityTags => utillityTags;
    public Enums.GroupTags[] GroupTags => groupTags;
    public Enums.GroupTags[] IncompatabillityTags => incompatabillityTags;
    public int Cost => cost;
}