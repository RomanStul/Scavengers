using Player.UI;
using ScriptableObjects.Material;
using UnityEngine;

namespace Player.Module
{
    public class HealthBar : Entities.HealthBar
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MaterialSO armorMaterial;
        //================================================================GETTER SETTER
        public override void SetModule(Module module)
        {
            ModuleRef = module;
        }

        //================================================================FUNCTIONALITY
        protected Player.Module.Module ModuleRef;
        
        
        public override void ApplyUpgrades()
        {
            if (ModuleRef.GetScript<Upgrades.Upgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.Upgrades.Ups.Armor))
            {
                material = armorMaterial;
            }

            if (healthBarConstants.maxAsStarting)
            {
                healthBarConstants.currentHealth = healthBarConstants.maxHealth;
            }
            ModuleRef.GetScript<Player.UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.maxHealth, UIController.BarsNames.HealthBar, true);
            ModuleRef.GetScript<Player.UI.UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
        }

        public override float TakeDamage(float damage, MaterialSO.DamageType damageType)
        {
            float health = base.TakeDamage(damage, damageType);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
            return health;
        }
    }
}
