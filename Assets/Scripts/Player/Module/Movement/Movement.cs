using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module.Movement
{
    public class Movement : Player.Module.ModuleBaseScript
    {
        //================================================================
        [SerializeField] protected Module.MovementConstants movementVariables;
        
        [SerializeField] protected Rigidbody2D rigid, rotationRigid;
        [SerializeField] protected GameObject moduleBody;
        //================================================================
        public float ThrustInput { get; set; } = 0f;
        public float RotationInput { get; set; } = 0f;
        //================================================================
        

        private void Update()
        {
            if (ThrustInput > 0)
            {
                rigid.AddForce(moduleBody.transform.up * (ThrustInput * movementVariables.Thrust));
                float speed = rigid.velocity.magnitude;
                if (speed > movementVariables.MaxSpeed)
                {
                    rigid.velocity = rigid.velocity.normalized * movementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                rotationRigid.AddTorque(-RotationInput * movementVariables.RotationThrust);
            }
        }
    }
}
