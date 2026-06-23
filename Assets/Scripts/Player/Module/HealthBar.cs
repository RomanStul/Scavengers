using System.Collections;
using Entities.Environment;
using Menu;
using Player.Module.Tools;
using Player.UI;
using ScriptableObjects.Material;
using ScriptableObjects.Tools;
using UnityEngine;

namespace Player.Module
{
    public class HealthBar : Entities.HealthBar
    {
        //================================================================CLASSES
        
        //================================================================EDITOR VARIABLES
        [SerializeField] protected MaterialSO armorMaterial;
        //================================================================GETTER SETTER

        private bool canTakeDamage = true;
        
        public override void SetModule(Module module)
        {
            ModuleRef = module;
        }

        public float GetMissingHealth()
        {
            //CAN be altered with upgrade for easier repairs
            return healthBarConstants.maxHealth - healthBarConstants.currentHealth;
        }

        public override void SetHealth(float health)
        {
            base.SetHealth(health);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
        }

        public void SetMinimalDamage(float damage)
        {
            minimalDamage = damage;
        }
        //================================================================FUNCTIONALITY
        protected Player.Module.Module ModuleRef;
        private float minimalDamage = 0f;
        
        
        public override void ApplyUpgrades()
        {
            if (ModuleRef.GetScript<Upgrades.ModuleUpgrades>(Module.ScriptNames.UpgradesScript).IsActive(Upgrades.ModuleUpgrades.Ups.Armor_I))
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
            if(SavesManager.Instance != null)
                damage *= SavesManager.Instance.GetDifficulty().damageTakenMultiplier;
            
            if (!canTakeDamage || damage < minimalDamage) return healthBarConstants.currentHealth;

            float health = base.TakeDamage(damage * Environment.instance.damageMultiplier, damageType);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
            canTakeDamage = false;
            StartCoroutine(InvincibilityTimer());

            if (health <= 0)
            {
                ToolHolder th = ModuleRef.GetScript<ToolHolder>(Module.ScriptNames.ToolScript);
                if (th.GetAmountOfToolType(ToolSO.ToolType.Repair_kit) > 0)
                {
                    th.UseTool(ToolSO.ToolType.Repair_kit);
                    return healthBarConstants.currentHealth;
                }
                else
                {
                    ModuleRef.Evacuate();
                    ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
                    return 0;
                }

            }
            return health;
        }

        private IEnumerator InvincibilityTimer()
        {
            yield return null;
            canTakeDamage = true;
        }

        public override void HealHealth(float health = -1)
        {
            base.HealHealth(health);
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).SetBar((int)healthBarConstants.currentHealth, UIController.BarsNames.HealthBar);
            ModuleRef.GetScript<ModuleSounds>(Module.ScriptNames.SoundsScript).PlaySound(ModuleSounds.SoundName.Repair, transform);
        }
    }
}
