using UnityEngine;

[CreateAssetMenu(fileName = "NodeFlamethrowerTurret", menuName = "Research Tree/Turrets/Flamethrower Turret Node")]
public class NodeFlamethrowerTurret : ProjectileTowerNode
{
    
    [SerializeField] private BurnEffect burnEffect;
    
    public override string TooltipText => "Flamethrower tower.";
    
    public override string GetStats(int rank)
    {
        if (_ProjectileTowerBlueprint != null)
        {
            return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
                   $"<b>Flamethrower (Rank {rank}):</b>\n";
        }
        return $"<size=120%><color=#FFD700>Cost: {Cost:F0}</color></size>\n\n" +
               "<color=#FF5555>Failed to load stats</color>";
    }
    
    public override void Initialize(int rank)
    {
        SetupNode(rank);
    }

    private void SetupNode(int rank)
    {
        CreateBlueprint();
        if (_ProjectileTowerBlueprint != null)
        {
            LoadBasicShot();
            LoadBasicStats(rank, 1.05f * rank);
            if (burnEffect)
            {
                _ProjectileTowerBlueprint.StatusEffects = new ResourceReference<StatusEffect>[] 
                {
                    new ResourceReference<StatusEffect> { Value = burnEffect }
                };
            }
        }
    }

    public override void LoadDependencies(int rank)
    {
        LoadBasicShot();
        if (_ProjectileTowerBlueprint != null)
        {
            BlueprintManager.InsertProjectileTowerBlueprint(_ProjectileTowerBlueprint);
        }
        if (burnEffect)
        {
            ResourceManager.RegisterStatusEffect(burnEffect.name, burnEffect);
        }
    }
}