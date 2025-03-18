using UnityEngine;

namespace Player.Module
{
    interface IBaseModule
    {
        void SetModule(Player.Module.Module module);
    }
    public class ModuleBaseScript : MonoBehaviour, IBaseModule
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        protected Player.Module.Module ModuleRef;
        //================================================================GETTER SETTER
        public virtual void SetModule(Player.Module.Module module)
        {
            this.ModuleRef = module;
        }
        //================================================================FUNCTIONALITY


    }
}
