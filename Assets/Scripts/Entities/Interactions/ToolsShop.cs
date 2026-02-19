using Player.Module;
using Player.Module.Tools;
using Player.Module.Upgrades;
using Player.UI;
using Player.UI.Tools;
using Module = Player.Module.Module;

namespace Entities.Interactions
{
    public class ToolsShop : Interactable
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void OpenToolsWindow(Module moduleRef)
        {
            UIWindow window = moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.Items);
            if (window != null)
            {
                ((ToolsShopWindow)window).SetStorage(moduleRef.GetScript<Storage>(Module.ScriptNames.StorageScript));
                ((ToolsShopWindow)window).SetToolHolder(moduleRef.GetScript<ToolHolder>(Module.ScriptNames.ToolScript));
                ((ToolsShopWindow)window).CreateToolButtons();
                ((ToolsShopWindow)window).UnlockTools(moduleRef.GetScript<ModuleUpgrades>(Module.ScriptNames.UpgradesScript).upgradesObject);
            }
        }
    }
}
