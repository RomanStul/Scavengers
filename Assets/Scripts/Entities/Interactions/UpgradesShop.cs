using Player.Module;
using Player.UI;
using Player.UI.Upgrades;
using UnityEngine;

namespace Entities.Interactions
{
    public class UpgradesShop : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void ApplyUpgrades(Module module)
        {
            UIWindow window = module.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.Upgrades);
            if (window != null)
            {
                ((UpgradeWindowController)window).SetStorage(module.GetScript<Storage>(Module.ScriptNames.StorageScript));
            }
        }
    }
}
