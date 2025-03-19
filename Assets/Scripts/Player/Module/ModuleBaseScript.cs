using System;
using UnityEngine;

namespace Player.Module
{
    public interface IBaseModule
    {
        void SetModule(Player.Module.Module module);
        void ApplyUpgrades();
    }
    public class ModuleBaseScript : BaseClass
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        protected Player.Module.Module ModuleRef;
        //================================================================GETTER SETTER
        public override void SetModule(Player.Module.Module module)
        {
            this.ModuleRef = module;
        }
        //================================================================FUNCTIONALITY

        public override void ApplyUpgrades()
        {
            
        }
    }
}
