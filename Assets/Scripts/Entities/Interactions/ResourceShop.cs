using Player.Module;
using Player.UI;
using UnityEngine;

namespace Entities.Interactions
{
    public class ResourceShop : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void OpenResourceShop(Module module)
        {
            module.GetScript<UIController>(Module.ScriptNames.UIControlsScript).ToggleResourceShop();
        }
    }
}
