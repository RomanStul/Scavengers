using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Module
{
    public class Input : Player.Module.ModuleBaseScript
    {
        
        public void RotationInput(InputAction.CallbackContext context)
        {
            ModuleRef.movementScript.RotationInput = context.ReadValue<Vector2>().x;
        }

        public void ThrustInput(InputAction.CallbackContext context)
        {
            ModuleRef.movementScript.ThrustInput = context.ReadValue<Vector2>().y;
        }
    }
}
