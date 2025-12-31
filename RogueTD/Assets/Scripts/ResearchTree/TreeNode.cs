using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NodeBasicTurret", menuName = "Research Tree/Nodes/Basic Turret")]
public abstract class TreeNode : ScriptableObject
{
    [SerializeField] protected string[] tags;
    [SerializeField] protected int maxRank = 9;
    [SerializeField] protected int minRank = 0;
    [SerializeField] protected TreeNode directUpgradeOf;
    [SerializeField] protected int cost;
    [SerializeField] protected Sprite icon;
    
    [SerializeField] protected string tooltipText = "Description failed to load properly";
    public virtual string TooltipText => tooltipText;
    public int MaxRank => maxRank;
    
    public int MinRank => minRank;
    public Sprite Icon => icon;
    public TreeNode DirectUpgradeOf => directUpgradeOf;
    public string[] Tags => tags;
    public int CurrentRank { get; set; } 
    public int Cost => cost;
    abstract public void OnActivate(int rank);
    abstract public void Initialize(int rank);
    abstract public string GetStats(int rank);
    abstract public int GetDynamicCost(int rank);
}