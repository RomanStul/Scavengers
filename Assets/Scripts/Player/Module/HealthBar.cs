using UnityEngine;

namespace Player.Module
{
    public class HealthBar : Entities.HealthBar
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        public override void SetModule(Module module)
        {
            ModuleRef = module;
        }

        //================================================================FUNCTIONALITY
        protected Player.Module.Module ModuleRef;
        
        
        public override void ApplyUpgrades()
        {
           //TODO swap for armored material instead of original 
        }
    }
}
