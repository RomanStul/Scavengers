using UnityEngine;

namespace Player.Module
{
    public class HealthBar : Entities.HealthBar, Player.Module.IBaseModule
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        public void SetModule(Module module)
        {
            ModuleRef = module;
        }

        //================================================================FUNCTIONALITY
        protected Player.Module.Module ModuleRef;
        
        
        public void ApplyUpgrades()
        {
            
        }
    }
}
