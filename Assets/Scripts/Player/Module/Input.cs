using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Module
{
    public class Input : Player.Module.ModuleBaseScript
    {
        
        public void RotationInput(InputAction.CallbackContext context)
        {
            ModuleRef.scripts.movementScript.RotationInput = context.ReadValue<Vector2>().x;
        }

        public void ThrustInput(InputAction.CallbackContext context)
        {
            ModuleRef.scripts.movementScript.ThrustInput = context.ReadValue<Vector2>().y;
        }

        public void DrillPositionInput(InputAction.CallbackContext context)
        {
            ModuleRef.scripts.drillScript.DrillPosition = context.ReadValue<Vector2>();
        }

        public void DrillUseInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ModuleRef.scripts.drillScript.UseDrill(true);
            }

            if (context.canceled)
            {
                ModuleRef.scripts.drillScript.UseDrill(false);
            }
        }
    }
}
