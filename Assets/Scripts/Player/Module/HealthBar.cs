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
        }
    }
}
