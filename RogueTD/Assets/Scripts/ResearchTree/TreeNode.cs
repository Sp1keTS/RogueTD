using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TreeNode", menuName = "Scriptable Objects/TreeNode")]
public abstract class TreeNode : ScriptableObject
{
    [SerializeField] private string[] tags;
    [SerializeField] protected int maxRank = 9;
    [SerializeField] protected int minRank = 0;
    [SerializeField] protected TreeNode directUpgradeOf;
    public int MaxRank => maxRank;
    public int MinRank => minRank;
    public TreeNode DirectUpgradeOf => directUpgradeOf;
    public string[] Tags => tags;
    public int Number { get; set; }
    public bool IsActive { get; set; } = false;
    public TreeNode[] PreviousNodes {get; set; }
    abstract public void OnActivate();
    abstract public void Initialize(int rank);
    abstract public void LoadDependencies();
}
