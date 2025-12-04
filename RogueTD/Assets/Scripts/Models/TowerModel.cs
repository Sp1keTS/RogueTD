using Unity.Mathematics;

namespace Models
{
    public class TowerModel
    {
        private float rotatingSpeed;
        private float targetingRange;
        private float damageMultiplier;
        private float attackSpeed;
        private int damage;
        private int projectileCount;
        private int maxAmmo;
        private float currentAmmo;
        private float ammoRegenerationSpeed;
        private ResourceReference<StatusEffect>[] statusEffects;
        
        public float RotatingSpeed
        {
            get => rotatingSpeed;
            set => rotatingSpeed = value < 0 ? 0 : value;
        }
        public float TargetingRange
        {
            get => targetingRange;
            set => targetingRange = value < 0 ? 0 : value;
        }
        public float DamageMultiplier
        {
            get => damageMultiplier;
            set => damageMultiplier = value < 0 ? 0 : value;
        }
        public float AttackSpeed
        {
            get => attackSpeed;
            set => attackSpeed = value < 0 ? 0 : value;
        }
        public int Damage
        {
            get => damage;
            set => damage = value < 0 ? 0 : value;
        }
        public int ProjectileCount
        {
            get => projectileCount;
            set => projectileCount = value < 0 ? 0 : value;
        }
        public int MaxAmmo
        {
            get => maxAmmo;
            set => maxAmmo = value < 0 ? 0 : value;
        }
        public float CurrentAmmo
        {
            get => currentAmmo;
            set => currentAmmo = math.clamp(value, 0, maxAmmo);
        }
        public float AmmoRegenerationSpeed
        {
            get => ammoRegenerationSpeed;
            set => ammoRegenerationSpeed = value < 0 ? 0 : value;
        }
        public ResourceReference<StatusEffect>[] StatusEffects { get => statusEffects;}
        
    }
}