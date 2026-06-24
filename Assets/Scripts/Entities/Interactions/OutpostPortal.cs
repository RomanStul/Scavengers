using Entities.Environment;
using Player.Module;
using Player.UI;
using Player.UI.PortalWindow;
using UnityEngine;

namespace Entities.Interactions
{
    public class OutpostPortal : SceneTransfer
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        private Module moduleRef;
        
        
        public void Use(Module module)
        {
            moduleRef = module;
            foreach (DestructionManager.PermanentObject permObject in DestructionManager.instance.GetPermanentObjects())
            {
                if (permObject.Type == DestructionManager.PermanentObject.permanentObjectType.GravityAnchor)
                {
                    PortalWindow port = (PortalWindow)module.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.Portal);
                    port.SetPortalRef(this);
                    return;
                }
            }
            
            base.LoadScene(module);
        }

        public void Transfer(string scene, Vector2 position)
        {
            base.LoadScene(moduleRef, position, scene);
            moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).CloseCurrentWindow();
        }
    }
}
