using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "Tower Defense/Blueprint")]
public class Blueprint : ScriptableObject
{
    [Header("Base Building Settings")]
    [SerializeField] protected GameObject mainObject;
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int cost;
    
    public GameObject MainObject => mainObject;
    public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }
    public int Cost { get => cost; set => cost = value; }
}
