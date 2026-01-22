using Player.UI;
using story;
using UnityEngine;

namespace Player.Module
{
    public class ModuleAnimations : ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY
        
        public void StartOfDayAnimationSetup()
        {
            ModuleRef.moveRb.linearVelocity = Vector2.zero;
            ModuleRef.moveRb.angularVelocity = 0f;
            transform.position = new Vector3(-45, 0, 0);
            transform.rotation = Convertor.RotationConversion(new Vector3(1, 0, 0), transform);
            ModuleRef.mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            StoryManager.instance.IncrementDay();
            
            (ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript)).SetNewDayNumber(StoryManager.instance.GetDayNumber());
            (ModuleRef.GetScript<Storage>(Module.ScriptNames.StorageScript)).PayWithCurrency((int)(StoryManager.instance.GetStartOfDayPayment()), true);
        }

        public void DisplayNews()
        {
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.News);
        }
    }
}
