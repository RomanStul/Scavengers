using Player.Module.Drill;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player.Module
{
    public class Input : Player.Module.ModuleBaseScript
    {
        //================================================================CLASSES
        //================================================================EDITOR VARIABLES
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY

        public void RotationInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).RotationInput = context.ReadValue<Vector2>().x;
        }

        public void ThrustInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).ThrustInput = context.ReadValue<Vector2>().y;
        }

        public void DrillPositionInput(InputAction.CallbackContext context)
        {
            ModuleRef.GetScript<DrillController>(Module.ScriptNames.DrillScript).DrillTargetPosition = context.ReadValue<Vector2>();
        }

        public void DrillUseInput(InputAction.CallbackContext context)
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

        public void DashInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).Dash();
            }
        }

        public void StopInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).Stop();
            }
        }

        public void DashRightInput(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).DashSideways(Vector2.right);
        }

        public void DashLeftInput(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            
            ModuleRef.GetScript<Movement.Movement>(Module.ScriptNames.MovementScript).DashSideways(Vector2.left);
        }
    }
}
