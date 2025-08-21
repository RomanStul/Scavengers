using Player.Module.Drill;
using Player.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;

namespace Player.Module
{
    public class Input : Player.Module.ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES

        private bool takeInput = true;
        
        //================================================================GETTER SETTER

        public void SetTakeInput(bool takeInputValue)
        {
            this.takeInput = takeInputValue;
        }
        
        //================================================================FUNCTIONALITY

        
        public void RotationInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).RotationInput = takeInput ? context.ReadValue<Vector2>().x : 0.0f;
        }

        public void ThrustInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).ThrustInput = takeInput ? context.ReadValue<Vector2>().y : 0.0f;
        }

        public void DrillPositionInput(InputAction.CallbackContext context)
        {
            if(takeInput)
                ModuleRef.GetScript<DrillController>(Module.ScriptNames.DrillScript).DrillTargetPosition = context.ReadValue<Vector2>();
                
        }

        public void DrillUseInput(InputAction.CallbackContext context)
        {
            if (takeInput)
            {
                if (context.started)
                {
                    ModuleRef.GetScript<DrillController>(Module.ScriptNames.DrillScript).UseDrill(true);
                }

                if (context.canceled)
                {
                    ModuleRef.GetScript<DrillController>(Module.ScriptNames.DrillScript).UseDrill(false);
                } 
            }
            else
            {
                ModuleRef.GetScript<DrillController>(Module.ScriptNames.DrillScript).UseDrill(false);
            }
        }

        public void DashInput(InputAction.CallbackContext context)
        {
            if (takeInput && context.started)
            {
                ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).Dash();
            }
        }

        public void StopInput(InputAction.CallbackContext context)
        {
            if (takeInput && context.started)
            {
                ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).Stop();
            }
        }

        public void MoveSidewaysInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript)
                .MoveSideways(takeInput ? context.ReadValue<Vector2>() : Vector2.zero);
        }

        public void InteractInput(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            
            ModuleRef.GetScript<InteractionHandler>(Module.ScriptNames.InteractionScript).UseEntity();
        }

        public void OpenInventoryInput(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.Inventory);
        }

        public void CloseWindow(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            
            ModuleRef.GetScript<UIController>(Module.ScriptNames.UIControlsScript).OpenWindow(UIController.WindowType.None);
        }
    }
}
