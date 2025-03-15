using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Module.Movement
{
    public class Movement : Player.Module.ModuleBaseScript
    {
        //================================================================
        [SerializeField] protected Module.MovementConstants movementVariables;
        
        [SerializeField] protected Rigidbody2D rigid;
        [SerializeField] protected GameObject moduleBodyRigid;
        //================================================================
        public float ThrustInput { get; set; } = 0f;
        public float RotationInput { get; set; } = 0f;
        //================================================================
        

        private void Update()
        {
            if (ThrustInput > 0)
            {
                rigid.AddForce(moduleBodyRigid.transform.up * (ThrustInput * movementVariables.Thrust));
                float speed = rigid.velocity.magnitude;
                if (speed > movementVariables.MaxSpeed)
                {
                    rigid.velocity = rigid.velocity.normalized * movementVariables.MaxSpeed;
                }
            }

            //TODO add speed build up and deceleration
            if (Mathf.Abs(RotationInput) > 0)
            {
                moduleBodyRigid.transform.rotation *= Quaternion.AngleAxis(RotationInput * movementVariables.RotationThrust, Vector3.forward);
            }
        }
    }
}
