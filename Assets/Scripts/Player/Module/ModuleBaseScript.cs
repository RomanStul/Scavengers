using UnityEngine;

namespace Player.Module
{
    interface IBaseModule
    {
        void SetModule(Player.Module.Module module);
    }
    public class ModuleBaseScript : MonoBehaviour, IBaseModule
    {
        //================================================================
        protected Player.Module.Module ModuleRef;
        //================================================================
        //================================================================
        public void SetModule(Player.Module.Module module)
        {
            this.ModuleRef = module;
        }
    }
}
