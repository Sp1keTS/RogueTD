using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public abstract class TreeNode 
{
    protected TreeNode directUpgradeOf;
    protected int cost;
    protected Sprite icon;
    protected string tooltipText = "Description failed to load properly";
    public virtual string TooltipText => tooltipText;
    public Sprite Icon => icon;
    public TreeNode DirectUpgradeOf => directUpgradeOf;
    public int CurrentRank { get; set; }
    public int Cost {get => cost; set => cost = value; }
    protected static float GetRankMultiplier(int rank) => 1 + rank * 0.1f;
    abstract public void OnActivate(int rank);
    abstract public string GetStats(int rank);
    abstract public int GetDynamicCost(int rank);
    
    public abstract List<Resource> GetResources();
}