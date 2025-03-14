using UnityEngine;

namespace Player.Module
{
    public class ModuleBaseScript : MonoBehaviour
    {
        protected Player.Module.Module ModuleRef;

        public void SetModule(Player.Module.Module module)
        {
            this.ModuleRef = module;
        }
    }
}
