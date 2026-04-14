using Player.Module;
using Player.Module.Tools;
using Player.UI;
using UnityEngine;

namespace Entities.Interactions
{
    public class Layer0Boulder : MonoBehaviour
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void ShowToolTutorial(Module moduleRef)
        {
            if (moduleRef.GetScript<ToolHolder>(Module.ScriptNames.ToolScript).ShowToolTutorial(true))
            {
                UIController uic = moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript);
                uic.SetHelpHideDelay(-1);
                uic.SetHelpWindowMode(HelpDisplay.DisplayModes.OnlyTools);
                uic.OpenWindow(UIController.WindowType.Help);
            }
            


        }

        public void HideToolTutorial(Module moduleRef)
        {
            moduleRef.GetScript<ToolHolder>(Module.ScriptNames.ToolScript).ShowToolTutorial(false);
            moduleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).CloseSpecificWindow(UIController.WindowType.Help);
        }
    }
}
