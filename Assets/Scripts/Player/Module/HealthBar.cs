using UnityEngine;

namespace Player.Module
{
    public class HealthBar : Entities.HealthBar, Player.Module.IBaseModule
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        protected Player.Module.Module ModuleRef;
        
        public void SetModule(Module module)
        {
            ModuleRef = module;
        }
    }
}
