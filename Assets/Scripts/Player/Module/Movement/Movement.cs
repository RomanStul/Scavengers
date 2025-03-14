using System;
using UnityEngine;

namespace Player.Module.Movement
{
    public class Movement : Player.Module.ModuleBaseScript
    {
        //================================================================
        [Serializable]
        protected class Constants
        {
            public float Thrust;
            public float RotationThrust;
            public float MaxSpeed;
        }

        //================================================================
        [SerializeField] protected Constants MovementVariables;
        
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
                rigid.AddForce(moduleBodyRigid.transform.up * (ThrustInput * MovementVariables.Thrust));
                float speed = rigid.velocity.magnitude;
                if (speed > MovementVariables.MaxSpeed)
                {
                    rigid.velocity = rigid.velocity.normalized * MovementVariables.MaxSpeed;
                }
            }

            if (Mathf.Abs(RotationInput) > 0)
            {
                moduleBodyRigid.transform.rotation *= Quaternion.AngleAxis(RotationInput * MovementVariables.RotationThrust, Vector3.forward);
            }
        }
    }
}
