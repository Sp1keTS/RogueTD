using UnityEngine;

[CreateAssetMenu(fileName = "BleedUpgrade", menuName = "Research Tree/Upgrades/Bleed Upgrade")]
public class BleedEffectUpgrade : ProjectileTowerUpgradeTreeNode
{
    [SerializeField] BleedEffect bleedEffect;
    
    public override void ApplyUpgrade(ProjectileTowerBlueprint blueprint, int rank)
    {
        if (bleedEffect == null)
        {
            Debug.LogError("BleedEffect is not assigned in BleedEffectUpgrade!");
            return;
        }
        
        ResourceManager.RegisterStatusEffect(bleedEffect.name, bleedEffect);
        
        if (blueprint.StatusEffects == null)
        {
            blueprint.StatusEffects = new ResourceReference<StatusEffect>[]
            {
                new ResourceReference<StatusEffect> { Value = bleedEffect }
            };
        }
        else
        {
            bool effectExists = false;
            foreach (var existingEffect in blueprint.StatusEffects)
            {
                if (existingEffect != null && existingEffect.Value != null && 
                    existingEffect.Value.name == bleedEffect.name)
                {
                    effectExists = true;
                    break;
                }
            }
            
            if (!effectExists)
            {
                var newEffects = new ResourceReference<StatusEffect>[blueprint.StatusEffects.Length + 1];
                blueprint.StatusEffects.CopyTo(newEffects, 0);
                newEffects[^1] = new ResourceReference<StatusEffect> { Value = bleedEffect };
                blueprint.StatusEffects = newEffects;
            }
        }
        
        Debug.Log($"Bleed effect '{bleedEffect.name}' added to blueprint '{blueprint.buildingName}'");
    }

    public override void LoadDependencies()
    {
        if (bleedEffect != null)
        {
            ResourceManager.RegisterStatusEffect(bleedEffect.name, bleedEffect);
        }
    }
}